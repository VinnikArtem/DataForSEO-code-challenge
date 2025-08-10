using System.IO.Compression;
using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing
{
    public class ArchiveExtractor : IArchiveExtractor
    {
        public async Task<string> ExtractAsync(string archivePath, string destinationDirectory)
        {
            if (!File.Exists(archivePath))
                throw new FileNotFoundException("Archive not found", archivePath);

            Directory.CreateDirectory(destinationDirectory);

            var fileName = Path.GetFileNameWithoutExtension(archivePath);

            var fullPath = Path.Combine(destinationDirectory, fileName);

            using var originalFileStream = File.OpenRead(archivePath);

            await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

            using var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress);

            decompressionStream.CopyTo(fileStream);

            return fullPath;
        }
    }
}
