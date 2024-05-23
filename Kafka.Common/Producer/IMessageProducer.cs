namespace Kafka.Common.Producer
{
    public interface IMessageProducer
    {
        Task ProduceAsync(string key, IMessage message, CancellationToken cancellationToken);
    }
}