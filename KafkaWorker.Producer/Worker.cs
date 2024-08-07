using Kafka.Common;
using Kafka.Common.Producer;

namespace KafkaWorker.Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageProducer _messageProducer;

        public Worker(ILogger<Worker> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"Worker for producer is running at: {DateTimeOffset.Now}");
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Starting to produce message at: {DateTimeOffset.Now}");
                var sampleMessage = new SampleMessage(Guid.NewGuid().ToString(), $"[from code] test message at {DateTimeOffset.Now}");
                await _messageProducer.ProduceAsync("PRACTICE", sampleMessage, stoppingToken);

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
