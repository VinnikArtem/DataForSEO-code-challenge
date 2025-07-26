using System.Net.Mime;

namespace Dispatcher.BLL.Strategies.ResponseDeserialization
{
    public class TextPlainResponseDeserializer : IResponseDeserializer
    {
        private static readonly char[] _separators = { '\r', '\n' };

        public string ContentType => MediaTypeNames.Text.Plain;

        public object Deserialize(string content, Type responseType)
        {
            return content.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
