using Dispatcher.DAL.Entities;

namespace Dispatcher.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<SuperTask> SuperTaskRepository { get; }

        IBaseRepository<FileProcessingTask> FileProcessingTaskRepository { get; }

        Task SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}
