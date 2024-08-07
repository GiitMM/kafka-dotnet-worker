using Kafka.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kafka.Common.Consumer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, params Assembly[] handlerAssemblies)
        {
            handlerAssemblies.Append(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(handlerAssemblies));

            // TODO: is there a way to avoid this? Better way to discover handlers?
            services.AddTransient<IKafkaMessageConsumerManager>(serviceProvider =>
                new KafkaMessageConsumerManager(serviceProvider, services));

            services.AddTransient<IKafkaConsumerBuilder, KafkaConsumerBuilder>();

            services.AddTransient<IKafkaTopicMessageConsumer, KafkaTopicMessageConsumer>();

            return services;
        }
    }
}