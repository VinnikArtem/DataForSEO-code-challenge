namespace TaskProcessor.Worker.Infrastructure.Models
{
    public class FileProcessingOptions
    {
        public int MaxParallelTasksCount { get; set; } = 5;
    }
}
