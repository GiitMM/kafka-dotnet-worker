using Confluent.Kafka;

namespace Kafka.Common.Consumer
{
    public interface IKafkaConsumerBuilder
    {
        IConsumer<string, string> Build();
    }
}