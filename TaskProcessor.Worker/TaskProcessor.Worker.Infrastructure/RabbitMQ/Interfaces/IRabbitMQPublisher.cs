namespace TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces
{
    public interface IRabbitMQPublisher : IAsyncDisposable
    {
        Task PublishMessageAsync<T>(T message, string queue);
    }
}
