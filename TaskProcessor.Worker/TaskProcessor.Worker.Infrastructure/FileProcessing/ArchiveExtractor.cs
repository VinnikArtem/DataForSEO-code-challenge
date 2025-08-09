using SharpCompress.Archives;
using System.Collections.Concurrent;
using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;

namespace TaskProcessor.Worker.Infrastructure.FileProcessing
{
    public class ArchiveExtractor : IArchiveExtractor
    {
        public async Task<IEnumerable<string>> ExtractAsync(string archivePath, string destinationDirectory)
        {
            if (!File.Exists(archivePath))
                throw new FileNotFoundException("Archive not found", archivePath);

            var filePaths = new ConcurrentBag<string>();

            Directory.CreateDirectory(destinationDirectory);

            using var archive = ArchiveFactory.Open(archivePath);

            foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
            {
                var fullPath = Path.Combine(destinationDirectory, entry.Key);

                filePaths.Add(fullPath);

                var directory = Path.GetDirectoryName(fullPath)!;
                Directory.CreateDirectory(directory);

                using var entryStream = entry.OpenEntryStream();

                await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

                await entryStream.CopyToAsync(fileStream);
            }

            return filePaths;
        }
    }
}
