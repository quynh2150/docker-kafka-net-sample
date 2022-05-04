using System;
using System.Text;
using Confluent.Kafka;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaNet
{
    public class ConsumerHostedService : IHostedService
    {
        private readonly ILogger<ConsumerHostedService> _logger;

        private readonly ClusterClient _cluster;

        private const string _topic = "j-topic";

        public ConsumerHostedService(ILogger<ConsumerHostedService> logger)
        {
            _logger = logger;
            _cluster =
                new ClusterClient(new Configuration
                {
                    Seeds = "localhost:9092"
                }, new ConsoleLogger());
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cluster.ConsumeFromLatest(topic: _topic);
            _cluster.MessageReceived += record =>
            {
                _logger.LogInformation($"Message received: {Encoding.UTF8.GetString(record.Value as byte[])}");
            };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
