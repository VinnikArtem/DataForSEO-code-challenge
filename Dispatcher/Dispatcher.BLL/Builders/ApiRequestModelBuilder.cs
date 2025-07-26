using Dispatcher.BLL.Models;

namespace Dispatcher.BLL.Builders
{
    public class ApiRequestModelBuilder
    {
        protected string _baseUrl;
        private ApiRequestModel _request;

        public ApiRequestModelBuilder(string hostUrl = "")
        {
            _baseUrl = hostUrl;
        }

        public ApiRequestModel Build()
        {
            var requestModel = _request.Clone();

            Clear();

            return (ApiRequestModel)requestModel;
        }

        public ApiRequestModelBuilder Create(string url, string method, CancellationTokenSource cancellationTokenSource)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("Url cannot be null or empty!");
            if (string.IsNullOrEmpty(method)) throw new ArgumentException("Method cannot be null or empty!");

            _request = new ApiRequestModel(cancellationTokenSource)
            {
                Url = new UriBuilder(string.Concat(_baseUrl, url)).Uri,
                Method = method
            };

            return this;
        }

        public void Clear()
        {
            _request = null;
        }

        public ApiRequestModelBuilder SetQueryString(string queryString)
        {
            _request.Url = new Uri(_request.Url.ToString() + queryString);

            return this;
        }
    }
}
