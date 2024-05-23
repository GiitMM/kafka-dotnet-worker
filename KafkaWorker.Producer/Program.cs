using Kafka.Common;
using Kafka.Common.Producer;
using KafkaWorker.Producer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddOptions<KafkaOptions>().Bind(builder.Configuration.GetSection("Kafka"));

builder.Services.AddKafkaProducer();

var host = builder.Build();
host.Run();
