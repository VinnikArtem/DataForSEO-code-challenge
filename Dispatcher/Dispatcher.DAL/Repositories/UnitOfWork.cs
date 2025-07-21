using Dispatcher.DAL.EF;
using Dispatcher.DAL.Entities;
using Dispatcher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dispatcher.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private IBaseRepository<SuperTask> _superTaskRepository;
        private IBaseRepository<FileProcessingTask> _fileProcessingTaskRepository;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IBaseRepository<SuperTask> SuperTaskRepository => _superTaskRepository ??= new BaseRepository<SuperTask>(_applicationContext);

        public IBaseRepository<FileProcessingTask> FileProcessingTaskRepository =>
            _fileProcessingTaskRepository ??= new BaseRepository<FileProcessingTask>(_applicationContext);

        public async Task BeginTransactionAsync()
        {
            _currentTransaction ??= await _applicationContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _applicationContext.SaveChangesAsync();
            await _currentTransaction.CommitAsync();

            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        public void Dispose()
        {
            _currentTransaction.Dispose();
            _currentTransaction?.Dispose();
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();

                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }
    }
}
