using System.Net.Mime;

namespace Dispatcher.BLL.Strategies.ResponseDeserialization
{
    public class TextPlainResponseDeserializer : IResponseDeserializer
    {
        public string ContentType => MediaTypeNames.Text.Plain;

        public object Deserialize(string content, Type? responseType = null)
        {
            var result = new List<string>();

            using (var reader = new StreamReader(content))
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        result.Add(line);
                    }
                }
            }

            return result;
        }
    }
}
