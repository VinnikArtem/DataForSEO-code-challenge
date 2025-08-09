using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces
{
    public interface IFileProcessingService
    {
        Task ProcessFileAsync(FileProcessingTaskRequest request);
    }
}
