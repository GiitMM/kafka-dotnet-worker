namespace Kafka.Common
{
    public class MessageTopicType
    {
        public MessageTopicType(string topic, Type messageType, int threads = 1)
        {
            Topic = topic;
            MessageType = messageType;
            Threads = threads;
        }

        public string Topic { get; }
        public Type MessageType { get; }
        public int Threads { get; }
    }
}
