using Microsoft.Extensions.Options;
using SharpCompress.Common;
using TaskProcessor.Worker.Infrastructure.Enums;
using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;
using TaskProcessor.Worker.Infrastructure.Models;
using TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IFileManager _fileManager;
        private readonly IArchiveExtractor _archiveExtractor;
        private readonly IFileAnalyzer _fileAnalyzer;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        private readonly SemaphoreSlim _parallelismLimiter;

        public FileProcessingService(
            IFileManager fileManager,
            IArchiveExtractor archiveExtractor,
            IFileAnalyzer fileAnalyzer,
            IRabbitMQPublisher rabbitMQPublisher,
            IOptions<FileProcessingOptions> options)
        {
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            _archiveExtractor = archiveExtractor ?? throw new ArgumentNullException(nameof(archiveExtractor));
            _fileAnalyzer = fileAnalyzer ?? throw new ArgumentNullException(nameof(fileAnalyzer));
            _rabbitMQPublisher = rabbitMQPublisher ?? throw new ArgumentNullException(nameof(rabbitMQPublisher));

            var maxParallelTasksCount = Math.Max(1, options.Value.MaxParallelTasksCount);

            _parallelismLimiter = new SemaphoreSlim(maxParallelTasksCount, maxParallelTasksCount);
        }

        public async Task ProcessFileAsync(FileProcessingTaskRequest request)
        {
            await _parallelismLimiter.WaitAsync();

            var archivePath = string.Empty;
            var folderPath = string.Empty;

            try
            {
                request.Status = FileProcessingTaskStatus.InProgress;

                await _rabbitMQPublisher.PublishMessageAsync(request, Constants.QueueNames.SubtaskUpdate);

                (archivePath, folderPath) = await _fileManager.DownloadAsync(request.LinkToFile);

                var archiveName = Path.GetFileName(archivePath);

                var filePath = await _archiveExtractor.ExtractAsync(archivePath, folderPath);

                request = await _fileAnalyzer.AnalyzeAsync(filePath, request);

                if (request.Status != FileProcessingTaskStatus.Error) request.Status = FileProcessingTaskStatus.Completed;

                await _rabbitMQPublisher.PublishMessageAsync(request, Constants.QueueNames.SubtaskUpdate);
            }
            finally
            {
                if (File.Exists(archivePath)) File.Delete(archivePath);
                if (Directory.Exists(folderPath)) Directory.Delete(folderPath, recursive: true);

                _parallelismLimiter.Release();
            }
        }
    }
}
