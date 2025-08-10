using Dispatcher.BLL.Models;
using Dispatcher.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Dispatcher.BLL.Consumers
{
    public class FileProcessingTaskUpdateConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRabbitMqConnectionManager _connectionManager;
        private readonly TaskCompletionSource _tcs = new();
        private readonly ILogger<FileProcessingTaskUpdateConsumer> _logger;
        private IChannel? _channel;

        public FileProcessingTaskUpdateConsumer(
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
                queue: Constants.QueueNames.SubtaskUpdate,
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

                    var fileProcessingTaskService = scope.ServiceProvider.GetRequiredService<IFileProcessingTaskService>();

                    var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var fileProcessingTaskRequest = JsonSerializer.Deserialize<FileProcessingTaskRequest>(messageBody);

                    if (fileProcessingTaskRequest != null)
                    {
                        await fileProcessingTaskService.UpdateAsync(fileProcessingTaskRequest);
                    }

                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong in FileProcessingTaskUpdateConsumer");

                    await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: Constants.QueueNames.SubtaskUpdate,
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
