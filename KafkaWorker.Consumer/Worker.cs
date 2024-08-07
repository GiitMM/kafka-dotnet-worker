using Kafka.Common.Consumer;

namespace KafkaWorker.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IKafkaMessageConsumerManager _kafkaMessageConsumerManager;

        public Worker(ILogger<Worker> logger, IKafkaMessageConsumerManager kafkaMessageConsumerManager)
        {
            _logger = logger;
            _kafkaMessageConsumerManager = kafkaMessageConsumerManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"Worker for consumer is running at: {DateTimeOffset.Now}");
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                await _kafkaMessageConsumerManager.StartConsumers(stoppingToken);
            }
        }
    }
}
