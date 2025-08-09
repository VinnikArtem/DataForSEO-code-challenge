using RabbitMQ.Client;

namespace TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces
{
    public interface IRabbitMqConnectionManager
    {
        bool IsConnected { get; }

        Task<IChannel> CreateChannelAsync();
    }
}
