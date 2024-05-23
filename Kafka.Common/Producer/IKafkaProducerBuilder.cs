using Confluent.Kafka;

namespace Kafka.Common.Producer
{
    public interface IKafkaProducerBuilder
    {
        IProducer<string, string> Build();
    }
}