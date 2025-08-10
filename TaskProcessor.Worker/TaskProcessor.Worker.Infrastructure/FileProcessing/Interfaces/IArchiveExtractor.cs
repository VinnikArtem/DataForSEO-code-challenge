namespace TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces
{
    public interface IArchiveExtractor
    {
        Task<string> ExtractAsync(string archivePath, string destinationDirectory);
    }
}
