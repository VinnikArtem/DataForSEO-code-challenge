using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace Dispatcher.BLL.Models
{
    public class ApiRequestModel : ICloneable
    {
        public string HttpClientName { get; set; }

        public string ContentType { get; set; } = MediaTypeNames.Application.Json;

        public string Content { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; }

        public CancellationToken CancellationToken => CancellationTokenSource.Token;

        public Dictionary<string, string> Headers { get; set; } = [];

        public string Method { get; set; } = HttpMethods.Get;

        public Uri Url { get; set; }

        public ApiRequestModel(CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
