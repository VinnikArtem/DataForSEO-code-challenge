using Dispatcher.BLL.Models;

namespace Dispatcher.BLL.Services.Interfaces
{
    public interface IApiService
    {
        Task<T> SendRequestAsync<T>(ApiRequestModel apiRequestModel);
    }
}
