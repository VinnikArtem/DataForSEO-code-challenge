namespace TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync<T>(T message, string queue);
    }
}
