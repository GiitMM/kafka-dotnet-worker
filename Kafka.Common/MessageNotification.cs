using MediatR;

namespace Kafka.Common
{
    public class MessageNotification<TMessage> : INotification
        where TMessage : class
    {
        public MessageNotification(TMessage message, string key)
        {
            Message = message;
            Key = key;
        }

        public TMessage Message { get; }
        public string Key { get; }
    }
}