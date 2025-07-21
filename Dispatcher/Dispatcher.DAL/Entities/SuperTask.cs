namespace Dispatcher.DAL.Entities
{
    public class SuperTask : BaseEntity
    {
        public ICollection<FileProcessingTask>? FileProcessingTasks { get; set; }
    }
}
