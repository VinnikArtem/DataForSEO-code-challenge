using Dispatcher.DAL.Enums;

namespace Dispatcher.BLL.Models
{
    public class FileProcessingTaskRequest
    {
        public string? LinkToFile { get; set; }

        public int LinesCount { get; set; }

        public long HighVolumeKeywordsCount { get; set; }

        public long MisspelledKeywordsCount { get; set; }

        public FileProcessingTaskStatus Status { get; set; }

        public Guid SuperTaskId { get; set; }
    }
}
