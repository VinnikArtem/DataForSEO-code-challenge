namespace TaskProcessor.Worker.Infrastructure.Models
{
    public class RabbitMQConnection
    {
        public string? HostName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }
}
