namespace TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces
{
    public interface IArchiveExtractor
    {
        Task<IEnumerable<string>> ExtractAsync(string archivePath, string destinationDirectory);
    }
}
