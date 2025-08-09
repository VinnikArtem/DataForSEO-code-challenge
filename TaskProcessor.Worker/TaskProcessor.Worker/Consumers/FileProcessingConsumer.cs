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
        private IFileProcessingService _fileProcessingService;
        private IChannel? _channel;

        public FileProcessingConsumer(
            IServiceScopeFactory scopeFactory,
            IRabbitMqConnectionManager connectionManager)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
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

            using var scope = _scopeFactory.CreateScope();

            _fileProcessingService = scope.ServiceProvider.GetRequiredService<IFileProcessingService>();

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
                    var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var superTaskRequest = JsonSerializer.Deserialize<SuperTaskRequest>(messageBody);

                    if (superTaskRequest?.FileProcessingTasks != null)
                    {
                        var tasks = superTaskRequest.FileProcessingTasks
                            .Select(request => _fileProcessingService.ProcessFileAsync(request));

                        await Task.WhenAll(tasks);
                    }

                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch
                {
                    await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: Infrastructure.Constants.QueueNames.RunSuperTask,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );
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
