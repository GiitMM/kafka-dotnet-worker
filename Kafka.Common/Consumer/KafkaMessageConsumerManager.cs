using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Kafka.Common.Consumer
{
    public class KafkaMessageConsumerManager : IKafkaMessageConsumerManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _services;

        public KafkaMessageConsumerManager(IServiceProvider serviceProvider, IServiceCollection services)
        {
            _serviceProvider = serviceProvider;
            _services = services;
        }

        public Task StartConsumers(CancellationToken cancellationToken, Dictionary<string, string> listTopic)
        {
            var topicsWithNotificationHandlers = GetTopicsWithNotificationHandlers(_services, listTopic);

            foreach (var topic in topicsWithNotificationHandlers)
            {
                var kafkaTopicMessageConsumer = _serviceProvider.GetRequiredService<IKafkaTopicMessageConsumer>();

                //kafkaTopicMessageConsumer.StartConsuming(topic, cancellationToken);
                new Thread(() => kafkaTopicMessageConsumer.StartConsuming(topic, cancellationToken)).Start();
            }
            return Task.CompletedTask;
        }

        private static IEnumerable<MessageTopicType> GetTopicsWithNotificationHandlers(IServiceCollection services, Dictionary<string, string> listTopic)
        {
            var messageTypesWithNotificationHandlers = services
                .Where(s => s.ServiceType.IsGenericType &&
                            s.ServiceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                .Select(s => s.ServiceType.GetGenericArguments()[0])
                .Where(s => s.IsGenericType &&
                            s.GetGenericTypeDefinition() == typeof(MessageNotification<>))
                .Select(s => s.GetGenericArguments()[0])
                .Where(s => typeof(IMessage).IsAssignableFrom(s))
                .Distinct();

            var groupedMessageTypes = messageTypesWithNotificationHandlers.GroupBy(g => g.UnderlyingSystemType);

            var processingData= groupedMessageTypes
                .Select(s => new
                {
                    Type = s.Key,
                    Attribute.GetCustomAttributes(s.Key).OfType<MessageTopicAttribute>().Where(s => s.MessageType == MessageBrokerType.KAFKA).FirstOrDefault()?.Topic,
                    Threads = Attribute.GetCustomAttributes(s.Key).OfType<MessageTopicAttribute>().Where(s => s.MessageType == MessageBrokerType.KAFKA).FirstOrDefault()?.Threads ?? 1
                })
                .Where(s => s.Topic is not null && s.Type is not null);

            return processingData.Select(s => new MessageTopicType(listTopic is not null && s.Topic is not null && listTopic.TryGetValue(s.Topic, out string? value) ? value : s.Topic!, s.Type, s.Threads));
        }
    }
}