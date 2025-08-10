namespace Dispatcher.BLL.Services.Interfaces
{
    public interface IRabbitMQPublisher : IAsyncDisposable
    {
        Task PublishMessageAsync<T>(T message, string queue);
    }
}
