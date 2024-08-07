using System;
using System.Net;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Kafka.Common.Producer
{
    public class KafkaProducerBuilder : IKafkaProducerBuilder
    {
        private readonly KafkaOptions _kafkaOptions;

        public KafkaProducerBuilder(IOptions<KafkaOptions> producerWorkerOptions)
        {
            _kafkaOptions = producerWorkerOptions?.Value ??
                            throw new ArgumentNullException(nameof(producerWorkerOptions));
        }

        public IProducer<string, string> Build()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaOptions.KafkaBootstrapServers,
                ClientId = Dns.GetHostName(),
            };

            var producerBuilder = new ProducerBuilder<string, string>(config);

            return producerBuilder.Build();
        }
    }
}