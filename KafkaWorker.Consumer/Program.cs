using Kafka.Common;
using Kafka.Common.Consumer;
using KafkaWorker.Consumer;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<KafkaOptions>().Bind(builder.Configuration.GetSection("Kafka"));

builder.Services.AddKafkaConsumer(Assembly.GetExecutingAssembly());

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
