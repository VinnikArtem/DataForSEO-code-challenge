using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces
{
    public interface IFileAnalyzer
    {
        Task<FileProcessingTaskRequest> AnalyzeAsync(string filePath, FileProcessingTaskRequest request);
    }
}
