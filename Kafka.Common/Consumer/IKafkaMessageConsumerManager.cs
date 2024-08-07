namespace Kafka.Common.Consumer
{
    public interface IKafkaMessageConsumerManager
    {
        Task StartConsumers(CancellationToken cancellationToken, Dictionary<string, string> listTopic = null);
    }
}