using Dispatcher.BLL.Models;

namespace Dispatcher.BLL.Services.Interfaces
{
    public interface ITaskService
    {
        Task CreateAndQueueSuperTaskAsync(TaskRequest taskRequest);
    }
}
