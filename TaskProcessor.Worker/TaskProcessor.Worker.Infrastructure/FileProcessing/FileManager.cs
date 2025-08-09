using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing
{
    public class FileManager : IFileManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public FileManager(IHttpClientFactory httpClientFactory, string httpClientName)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClientName = httpClientName ?? throw new ArgumentNullException(nameof(httpClientName));
        }

        public async Task<(string FilePath, string DirectoryPath)> DownloadAsync(string url, string? destinationPath = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url);

            destinationPath = GetDestinationPath(destinationPath);

            var fileName = Path.GetFileName(new Uri(url).LocalPath);
            var filePath = Path.Combine(destinationPath, fileName);

            Directory.CreateDirectory(destinationPath);

            var httpClient = _httpClientFactory.CreateClient(_httpClientName);

            await using var fs = new FileStream(filePath, FileMode.Create);

            using var stream = await httpClient.GetStreamAsync(url);

            await stream.CopyToAsync(fs);

            return (filePath, destinationPath);
        }

        private string GetDestinationPath(string destinationPath)
        {
            var folderId = Guid.NewGuid().ToString();

            return string.IsNullOrWhiteSpace(destinationPath)
                ? Path.Combine(Path.GetTempPath(), folderId)
                : Path.Combine(destinationPath, folderId);
        }
    }
}
