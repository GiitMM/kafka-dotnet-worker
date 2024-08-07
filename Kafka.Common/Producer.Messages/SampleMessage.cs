namespace Kafka.Common
{
    [MessageTopic("practice-kafka-dotnet", MessageBrokerType.KAFKA)]
    public class SampleMessage : IMessage
    {
        public SampleMessage(string messageId, string messageValue)
        {
            MessageId = messageId;
            MessageValue = messageValue;
        }

        public string MessageId { get; }

        public string MessageValue { get; }
    }
}