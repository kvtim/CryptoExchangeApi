using Confluent.Kafka;
using EventBus.Messages.Events;
using Logger.Logger;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logger.KafkaConsumer
{
    public class KafkaConsumerHandler
    {
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly IElasticLogger _logger;

        public KafkaConsumerHandler(
            string bootstrapServers, 
            string groupId,
            IElasticLogger logger)
        {
            _bootstrapServers = bootstrapServers;
            _groupId = groupId;
            _logger = logger;
        }


        public async Task StartAsync(string topic)
        {
            var config = new ConsumerConfig
            {
                GroupId = _groupId,
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
                consumerBuilder.Subscribe(topic);

                var cancelToken = new CancellationTokenSource();

                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);

                    var messageValue = consumer.Message.Value;

                    if (messageValue != null)
                    {
                        var log = JsonSerializer.Deserialize<CreateNewLogEvent>(messageValue);

                        await _logger.AddOrUpdateLog(
                            log.Microservice,
                            log.LogType,
                            log.Message,
                            log.LogTime);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine(ex.Message); 
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
    }
}
