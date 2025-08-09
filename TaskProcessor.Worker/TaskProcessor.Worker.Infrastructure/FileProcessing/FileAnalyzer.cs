using TaskProcessor.Worker.Infrastructure.Enums;
using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;
using TaskProcessor.Worker.Infrastructure.Models;
using TaskProcessor.Worker.Infrastructure.Strategies.FileParsing;
using TaskProcessor.Worker.Infrastructure.Strategies.MetricCalculation;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing
{
    public class FileAnalyzer : IFileAnalyzer
    {
        private readonly IEnumerable<IMetricCalculator> _metricCalculators;
        private readonly IEnumerable<IFileParser> _fileParsers;
        private readonly object _lock = new();

        public FileAnalyzer(IEnumerable<IMetricCalculator> metricCalculators, IEnumerable<IFileParser> fileParsers)
        {
            _metricCalculators = metricCalculators ?? throw new ArgumentNullException(nameof(metricCalculators));
            _fileParsers = fileParsers ?? throw new ArgumentNullException(nameof(fileParsers));
        }

        public async Task<FileProcessingTaskRequest> AnalyzeAsync(string filePath, FileProcessingTaskRequest request)
        {
            try
            {
                var fileExtension = Path.GetExtension(filePath);

                var parser = _fileParsers.FirstOrDefault(p => p.FileType == fileExtension);

                await foreach (var keyword in parser.ParseAsync<Keyword>(filePath))
                {
                    try
                    {
                        foreach (var metricCalculator in _metricCalculators)
                        {
                            metricCalculator.Calculate(keyword.DeserializedObject, request);
                        }
                    }
                    catch
                    {
                        request.InvalidLines.Add(keyword.LineNumber);
                    }
                }
            }
            catch
            {
                lock (_lock)
                {
                    request.IsFileCorrupted = true;
                    request.Status = FileProcessingTaskStatus.Error;
                }
            }

            return request;
        }
    }
}
