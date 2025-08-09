using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.Strategies.FileParsing
{
    public interface IFileParser
    {
        string FileType { get; }

        IAsyncEnumerable<(T DeserializedObject, int LineNumber)> ParseAsync<T>(string filePath);
    }
}
