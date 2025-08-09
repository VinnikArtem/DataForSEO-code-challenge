using System.Text.Json;

namespace TaskProcessor.Worker.Infrastructure.Strategies.FileParsing
{
    public class JsonParser : IFileParser
    {
        public string FileType => Constants.FileTypes.Json;

        public async IAsyncEnumerable<(T DeserializedObject, int LineNumber)> ParseAsync<T>(string filePath)
        {
            using var reader = new StreamReader(filePath);

            string? line;
            var lineNumber = 0;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber += 1;

                yield return (JsonSerializer.Deserialize<T>(line), lineNumber);
            }
        }
    }
}
