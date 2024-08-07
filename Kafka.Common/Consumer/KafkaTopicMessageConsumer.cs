using System;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kafka.Common.Consumer
{
    public class KafkaTopicMessageConsumer : IKafkaTopicMessageConsumer
    {
        private readonly IKafkaConsumerBuilder _kafkaConsumerBuilder;
        private readonly ILogger<KafkaTopicMessageConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public KafkaTopicMessageConsumer(ILogger<KafkaTopicMessageConsumer> logger,
            IKafkaConsumerBuilder kafkaConsumerBuilder, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _kafkaConsumerBuilder = kafkaConsumerBuilder;
            _serviceProvider = serviceProvider;
        }

        public async Task StartConsuming(MessageTopicType topic, CancellationToken cancellationToken)
        {
            using (var consumer = _kafkaConsumerBuilder.Build())
            {
                //_logger.LogInformation($"Starting consumer for {topic.Topic}");
                consumer.Subscribe(topic.Topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        // TODO: log error if missing header
                        //var messageTypeEncoded = consumeResult.Message.Headers.GetLastBytes("message-type");
                        //var messageTypeHeader = Encoding.UTF8.GetString(messageTypeEncoded);
                        var messageType = Type.GetType(topic.MessageType.AssemblyQualifiedName);

                        var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, messageType);
                        var messageNotificationType = typeof(MessageNotification<>).MakeGenericType(messageType);
                        var messageNotification = Activator.CreateInstance(messageNotificationType, message, consumeResult.Message.Key);
                        var messageValue = consumeResult.Message.Value;

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await mediator.Publish(messageNotification, cancellationToken);
                            consumer.Commit(consumeResult);
                        }

                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("The Kafka consumer thread has been cancelled");
                        break;
                    }
                    catch (ConsumeException ce)
                    {
                        _logger.LogError($"Consume error: {ce.Error.Reason}");
                        if (ce.Error.IsFatal)
                        {
                            _logger.LogError(ce, ce.Message);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        consumer.Close();
                        consumer.Dispose();
                    }
                    //finally
                    //{
                    //    consumer.Close();
                    //}
                }
            }
        }
    }
}