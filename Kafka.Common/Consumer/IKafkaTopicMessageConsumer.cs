namespace Kafka.Common.Consumer
{
    public interface IKafkaTopicMessageConsumer
    {
        Task StartConsuming(MessageTopicType topic, CancellationToken cancellationToken);
    }
}