using Dispatcher.BLL.Models;

namespace Dispatcher.BLL.Services.Interfaces
{
    public interface IFileProcessingTaskService
    {
        Task<FileProcessingTaskRequest> GetFileProcessingTaskByIdAsync(Guid id);

        Task UpdateAsync(FileProcessingTaskRequest request);
    }
}
