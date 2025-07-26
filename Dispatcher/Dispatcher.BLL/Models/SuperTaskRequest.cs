namespace Dispatcher.BLL.Models
{
    public class SuperTaskRequest
    {
        public IEnumerable<FileProcessingTaskRequest> FileProcessingTasks { get; set; }
    }
}
