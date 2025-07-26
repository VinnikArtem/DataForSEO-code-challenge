using Dispatcher.BLL.Models;
using Dispatcher.BLL.Services.Interfaces;
using Dispatcher.BLL.Strategies.ResponseDeserialization;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Dispatcher.BLL.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEnumerable<IResponseDeserializer> _responseDeserializers;
        private readonly string _httpClientName;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            IEnumerable<IResponseDeserializer> responseDeserializers,
            string httpClientName)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _responseDeserializers = responseDeserializers ?? throw new ArgumentNullException(nameof(responseDeserializers));
            _httpClientName = httpClientName ?? throw new ArgumentNullException(nameof(httpClientName));
        }

        public async Task<T> SendRequestAsync<T>(ApiRequestModel apiRequestModel)
        {
            ArgumentNullException.ThrowIfNull(apiRequestModel);

            var requestMessage = CreateRequestMessage(apiRequestModel);

            var response = await _httpClientFactory
                .CreateClient(GetHttpClientName(apiRequestModel))
                .SendAsync(requestMessage, apiRequestModel.CancellationToken);

            if (!response.IsSuccessStatusCode) return default;

            var responseDeserializer = _responseDeserializers.FirstOrDefault(rd => response.Content?.Headers?.ContentType?.MediaType == rd.ContentType);

            if (responseDeserializer == null) return default;

            var content = await response.Content.ReadAsStringAsync();

            return (T)responseDeserializer.Deserialize(content, typeof(T));
        }

        private HttpRequestMessage CreateRequestMessage(ApiRequestModel apiRequestModel)
        {
            ArgumentNullException.ThrowIfNull(apiRequestModel);
            ArgumentNullException.ThrowIfNull(apiRequestModel.Url);

            var requestMessage = new HttpRequestMessage(
                new HttpMethod(apiRequestModel.Method),
                apiRequestModel.Url);

            if (apiRequestModel.Method != HttpMethods.Get && apiRequestModel.Method != HttpMethods.Delete)
            {
                requestMessage.Content = new StringContent(
                    apiRequestModel.Content,
                    Encoding.UTF8,
                    apiRequestModel.ContentType);
            }

            foreach (var (key, value) in apiRequestModel.Headers)
            {
                requestMessage.Headers.Add(key, value);
            }

            return requestMessage;
        }

        private string GetHttpClientName(ApiRequestModel apiRequestModel)
        {
            return !string.IsNullOrEmpty(apiRequestModel.HttpClientName) ? apiRequestModel.HttpClientName : _httpClientName;
        }
    }
}
