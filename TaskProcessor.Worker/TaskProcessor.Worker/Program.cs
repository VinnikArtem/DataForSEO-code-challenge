using TaskProcessor.Worker.Consumers;
using TaskProcessor.Worker.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices(builder.Configuration);

builder.Services.AddHostedService<FileProcessingConsumer>();

var host = builder.Build();
host.Run();
