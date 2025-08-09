namespace TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces
{
    public interface IFileManager
    {
        Task<(string FilePath, string DirectoryPath)> DownloadAsync(string url, string? destinationPath = null);
    }
}
