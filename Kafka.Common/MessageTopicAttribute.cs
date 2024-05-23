namespace Kafka.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class MessageTopicAttribute : Attribute
    {
        public MessageTopicAttribute(string topic, string messageType, int threads = 1)
        {
            Topic = topic;
            MessageType = messageType;
            Threads = threads;
        }

        public string Topic { get; }
        public string MessageType { get; }
        public int Threads { get; }
    }
}