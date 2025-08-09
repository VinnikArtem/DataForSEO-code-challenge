using System.Collections.Concurrent;
using TaskProcessor.Worker.Infrastructure.Enums;

namespace TaskProcessor.Worker.Infrastructure.Models
{
    public class FileProcessingTaskRequest
    {
        public Guid Id { get; set; }

        public string? LinkToFile { get; set; }

        public int LinesCount { get; set; }

        public long HighVolumeKeywordsCount { get; set; }

        public long MisspelledKeywordsCount { get; set; }

        public FileProcessingTaskStatus Status { get; set; }

        public Guid SuperTaskId { get; set; }

        public ConcurrentBag<int> InvalidLines { get; } = [];

        public bool IsFileCorrupted { get; set; }
    }
}
