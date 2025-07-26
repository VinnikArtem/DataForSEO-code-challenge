namespace Dispatcher.BLL.Strategies.ResponseDeserialization
{
    public interface IResponseDeserializer
    {
        string ContentType { get; }

        object Deserialize(string content, Type? responseType = null);
    }
}
