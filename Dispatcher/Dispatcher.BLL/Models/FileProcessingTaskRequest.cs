using Dispatcher.DAL.Enums;

namespace Dispatcher.BLL.Models
{
    public class FileProcessingTaskRequest
    {
        public Guid Id { get; set; }

        public string? LinkToFile { get; set; }

        public int LinesCount { get; set; }

        public long HighVolumeKeywordsCount { get; set; }

        public long MisspelledKeywordsCount { get; set; }

        public FileProcessingTaskStatus Status { get; set; }

        public bool IsFileCorrupted { get; set; }

        public IEnumerable<int> InvalidLines { get; set; } = [];

        public Guid SuperTaskId { get; set; }
    }
}
