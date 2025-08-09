using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskProcessor.Worker.Infrastructure.Models;
using TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces;

namespace TaskProcessor.Worker.Infrastructure.RabbitMQ
{
    public class RabbitMqConnectionManager : IRabbitMqConnectionManager
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly RabbitMQConnection _rabbitMQConnection;
        private IConnection? _connection;
        private bool _disposed;

        public RabbitMqConnectionManager(IOptions<RabbitMQConnection> rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection?.Value ?? throw new ArgumentNullException(nameof(rabbitMQConnection));
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public async Task<IChannel> CreateChannelAsync()
        {
            if (!IsConnected)
            {
                await TryConnect();
            }

            if (_connection == null)
                throw new InvalidOperationException("RabbitMQ connection is not established.");

            return await _connection.CreateChannelAsync();
        }

        private async Task TryConnect()
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                if (IsConnected) return;

                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMQConnection.HostName,
                    UserName = _rabbitMQConnection.UserName,
                    Password = _rabbitMQConnection.Password
                };

                _connection = await factory.CreateConnectionAsync();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _disposed = true;

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }

            _semaphoreSlim.Dispose();
        }
    }
}
