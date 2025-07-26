using AutoMapper;
using Dispatcher.BLL.Builders;
using Dispatcher.BLL.Models;
using Dispatcher.BLL.Services.Interfaces;
using Dispatcher.DAL.Entities;
using Dispatcher.DAL.Enums;
using Dispatcher.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Dispatcher.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApiService _apiService;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IApiService apiService, IRabbitMQPublisher rabbitMQPublisher, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _rabbitMQPublisher = rabbitMQPublisher ?? throw new ArgumentNullException(nameof(rabbitMQPublisher));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateAndQueueSuperTaskAsync(TaskRequest taskRequest)
        {
            using var cancellationToken = new CancellationTokenSource();

            var apiRequest = new ApiRequestModelBuilder()
                .Create(taskRequest.Link, HttpMethods.Get, cancellationToken)
                .Build();

            var fileLinks = await _apiService.SendRequestAsync<IEnumerable<string>>(apiRequest);

            if (fileLinks is null || !fileLinks.Any()) return;

            var superTask = new SuperTask
            {
                FileProcessingTasks = fileLinks.Select(link => new FileProcessingTask
                {
                    LinkToFile = link,
                    Status = FileProcessingTaskStatus.NotStarted
                }).ToList()
            };

            await _unitOfWork.SuperTaskRepository.CreateAsync(superTask);
            await _unitOfWork.SaveChangesAsync();

            var superTaskRequest = _mapper.Map<SuperTaskRequest>(superTask);

            await _rabbitMQPublisher.PublishMessageAsync(superTaskRequest, Constants.QueueNames.RunSuperTask);
        }
    }
}
