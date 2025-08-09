namespace TaskProcessor.Worker.Infrastructure.Models
{
    public class SuperTaskRequest
    {
        public IEnumerable<FileProcessingTaskRequest>? FileProcessingTasks { get; set; }
    }
}
