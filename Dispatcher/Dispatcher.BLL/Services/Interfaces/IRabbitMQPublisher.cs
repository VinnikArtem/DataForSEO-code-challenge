namespace Dispatcher.BLL.Services.Interfaces
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync<T>(T message, string queue);
    }
}
