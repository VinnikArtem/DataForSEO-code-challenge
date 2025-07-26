using RabbitMQ.Client;

namespace Dispatcher.BLL.Services.Interfaces
{
    public interface IRabbitMqConnectionManager : IAsyncDisposable
    {
        bool IsConnected { get; }

        Task<IChannel> CreateChannelAsync();
    }
}
