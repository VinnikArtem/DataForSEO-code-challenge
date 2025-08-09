using Dispatcher.BLL.Mappers;
using Dispatcher.BLL.Models;
using Dispatcher.BLL.Services;
using Dispatcher.BLL.Services.Interfaces;
using Dispatcher.BLL.Strategies.ResponseDeserialization;
using Dispatcher.DAL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dispatcher.BLL.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDALServices(configuration);

            services.Configure<RabbitMQConnection>(configuration.GetSection("RabbitMQ"));

            services.AddTransient<IResponseDeserializer, TextPlainResponseDeserializer>();

            services.AddHttpClient();

            services.AddTransient<IApiService>(x => new ApiService(
                x.GetService<IHttpClientFactory>(),
                x.GetServices<IResponseDeserializer>(),
                "DispatcherClient"));

            services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();
            services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

            services.AddAutoMapper(cfg => { }, typeof(SuperTaskProfile), typeof(FileProcessingTaskProfile));

            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IFileProcessingTaskService, FileProcessingTaskService>();
        }
    }
}
