using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskProcessor.Worker.Infrastructure.FileProcessing;
using TaskProcessor.Worker.Infrastructure.FileProcessing.Interfaces;
using TaskProcessor.Worker.Infrastructure.Models;
using TaskProcessor.Worker.Infrastructure.RabbitMQ;
using TaskProcessor.Worker.Infrastructure.RabbitMQ.Interfaces;
using TaskProcessor.Worker.Infrastructure.Strategies.FileParsing;
using TaskProcessor.Worker.Infrastructure.Strategies.MetricCalculation;

namespace TaskProcessor.Worker.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileProcessingOptions>(configuration.GetSection("FileProcessing"));
            services.Configure<RabbitMQConnection>(configuration.GetSection("RabbitMQ"));

            services.AddTransient<IMetricCalculator, LinesCountMetricCalculator>();
            services.AddTransient<IMetricCalculator, HighVolumeKeywordsCountMetricCalculator>();
            services.AddTransient<IMetricCalculator, MisspelledKeywordsCountMetricCalculator>();

            services.AddScoped<IFileParser, JsonParser>();

            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IArchiveExtractor, ArchiveExtractor>();
            services.AddScoped<IFileAnalyzer, FileAnalyzer>();

            services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();
            services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

            services.AddSingleton<IFileProcessingService, FileProcessingService>();
        }
    }
}
