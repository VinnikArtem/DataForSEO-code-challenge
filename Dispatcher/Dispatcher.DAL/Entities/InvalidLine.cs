namespace Dispatcher.DAL.Entities
{
    public class InvalidLine : BaseEntity
    {
        public int LineNumber { get; set; }

        public Guid FileProcessingTaskId { get; set; }

        public FileProcessingTask? FileProcessingTask { get; set; }
    }
}
