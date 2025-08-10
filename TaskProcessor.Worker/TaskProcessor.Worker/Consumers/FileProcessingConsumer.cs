using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;
using TaskProcessor.Worker.Infrastructure.Models;
using TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces;

namespace TaskProcessor.Worker.Consumers
{
    public class FileProcessingConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRabbitMqConnectionManager _connectionManager;
        private readonly TaskCompletionSource _tcs = new();
        private readonly ILogger<FileProcessingConsumer> _logger;
        private IChannel? _channel;

        public FileProcessingConsumer(
            IServiceScopeFactory scopeFactory,
            IRabbitMqConnectionManager connectionManager,
            ILogger<FileProcessingConsumer> logger)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = await _connectionManager.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: Infrastructure.Constants.QueueNames.RunSuperTask,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel == null)
                throw new InvalidOperationException("RabbitMQ channel not initialized.");

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    await using var scope = _scopeFactory.CreateAsyncScope();

                    var fileProcessingService = scope.ServiceProvider.GetRequiredService<IFileProcessingService>();

                    var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var superTaskRequest = JsonSerializer.Deserialize<SuperTaskRequest>(messageBody);

                    if (superTaskRequest?.FileProcessingTasks != null)
                    {
                        var tasks = superTaskRequest.FileProcessingTasks
                            .Select(request => fileProcessingService.ProcessFileAsync(request));

                        await Task.WhenAll(tasks);
                    }

                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong in FileProcessingConsumer");

                    await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: Infrastructure.Constants.QueueNames.RunSuperTask,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

            await _tcs.Task;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
