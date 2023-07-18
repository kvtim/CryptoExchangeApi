using Confluent.Kafka;
using UserManagement.Core.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Services.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IConfiguration _configuration;

        private readonly ProducerConfig _config;

        public KafkaProducerService(IConfiguration configuration)
        {
            _configuration = configuration;

            _config = new ProducerConfig
            {
                BootstrapServers = _configuration.GetSection("KafkaSettings:HostAddress").Value
            };
        }

        public async Task<bool> SendMessage(string topic, string message)
        {
            try
            {
                using var producer = new ProducerBuilder<Null, string>(_config).Build();

                var result = await producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = message
                });

                Console.WriteLine($"Delivery Timestamp:{result.Timestamp.UtcDateTime}");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Kafka exception: {ex.Message}");
            }
        }
    }
}
