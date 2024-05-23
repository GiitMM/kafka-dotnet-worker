using Kafka.Common;
using MediatR;

namespace KafkaWorker.Consumer
{
    public class SampleMessageLoggerHandler : INotificationHandler<MessageNotification<SampleMessage>>
    {
        private readonly ILogger<SampleMessageLoggerHandler> _logger;

        public SampleMessageLoggerHandler(ILogger<SampleMessageLoggerHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MessageNotification<SampleMessage> notification, CancellationToken cancellationToken)
        {
            var message = notification.Message;

            _logger.LogInformation(
                $"[Thread: {Environment.CurrentManagedThreadId}] Sample message received with id: {message.MessageId} and value: {message.MessageValue}");

            return Task.CompletedTask;
        }
    }
}