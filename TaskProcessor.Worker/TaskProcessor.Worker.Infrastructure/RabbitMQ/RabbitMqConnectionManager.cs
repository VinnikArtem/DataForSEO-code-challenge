using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces;

namespace TaskProcessor.Worker.Infrastructure.RabbitMQ
{
    public class RabbitMqConnectionManager : IRabbitMqConnectionManager
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private bool _disposed;

        public RabbitMqConnectionManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                    HostName = _configuration["RabbitMQ:HostName"],
                    UserName = _configuration["RabbitMQ:UserName"],
                    Password = _configuration["RabbitMQ:Password"]
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
