using System;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaNet
{
    public class ProducerHostedService : IHostedService
    {
        private readonly ILogger<ProducerHostedService> _logger;

        private readonly IProducer<Null, string> _producer;

        private const string _topic = "j-topic";

        public ProducerHostedService(ILogger<ProducerHostedService> logger)
        {
            _logger = logger;
            var config =
                new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                var msg = $"Produce message: {i}";
                _logger.LogInformation (msg);
                await _producer
                    .ProduceAsync(topic: _topic,
                    message: new Message<Null, string> { Value = msg },
                    cancellationToken);
            }
            _producer.Flush(timeout: TimeSpan.FromSeconds(100));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
