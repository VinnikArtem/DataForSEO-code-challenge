using Dispatcher.BLL.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Dispatcher.BLL.Services
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IRabbitMqConnectionManager _connectionManager;
        private IChannel? _channel;

        public RabbitMQPublisher(IRabbitMqConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public async Task PublishMessageAsync<T>(T message, string queue)
        {
            if (_channel == null || !_connectionManager.IsConnected)
            {
                _channel = await _connectionManager.CreateChannelAsync();
            }

            await _channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var props = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, mandatory: true, basicProperties: props, body: body);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
        }
    }
}
