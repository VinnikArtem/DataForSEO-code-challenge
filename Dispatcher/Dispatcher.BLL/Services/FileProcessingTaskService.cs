using AutoMapper;
using Dispatcher.BLL.Models;
using Dispatcher.BLL.Services.Interfaces;
using Dispatcher.DAL.Entities;
using Dispatcher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.BLL.Services
{
    public class FileProcessingTaskService : IFileProcessingTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileProcessingTaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<FileProcessingTaskRequest> GetFileProcessingTaskByIdAsync(Guid id)
        {
            var subtask = await _unitOfWork.FileProcessingTaskRepository.GetFirstOrDefaultAsync(
                t => t.Id == id,
                query => query.Include(t => t.InvalidLines));

            if (subtask == null) return default;

            return _mapper.Map<FileProcessingTaskRequest>(subtask);
        }

        public async Task UpdateAsync(FileProcessingTaskRequest request)
        {
            if (request == null) return;

            var subtask = _mapper.Map<FileProcessingTask>(request);

            await _unitOfWork.FileProcessingTaskRepository.UpdateAsync(subtask);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
