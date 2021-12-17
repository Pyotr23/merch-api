using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseApi.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseApi.Infrastructure.MessageBroker
{
    public class KafkaManager
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<KafkaManager> _logger;
        
        public KafkaConfiguration Configuration { get; }
        
        public KafkaManager(IOptions<KafkaConfiguration> options,
            ILogger<KafkaManager> logger)
        {
            Configuration = options.Value;
            _consumerConfig = new ConsumerConfig
            {
                GroupId = Configuration.GroupId,
                BootstrapServers = Configuration.BootstrapServers
            };
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = Configuration.BootstrapServers
            };
            _logger = logger;
        }

        public async Task ProduceAsync(string topic, string key, object value, CancellationToken token)
        {
            var producer = new ProducerBuilder<string, string>(_producerConfig).Build();
            
            var message = new Message<string, string>
            {
                Key = key,
                Value = JsonSerializer.Serialize(value)
            };
            
            await producer.ProduceAsync(topic, message, token);
        }
        
        public async Task StartConsuming(string topic, 
            IServiceScopeFactory scopeFactory, 
            Func<string, CancellationToken, Task> processMessage,
            CancellationToken token)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe(topic);
            try
            {
                while (!token.IsCancellationRequested)
                {
                    using var scope = scopeFactory.CreateScope();
                    try
                    {
                        await Task.Yield();
                        var consumeResult = consumer.Consume(token);
                        if (consumeResult is null)
                            continue;

                        await processMessage(consumeResult.Message.Value, token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Consume topic {name} error: {error}", topic, ex.Message);
                    }
                }
            }
            finally
            {
                consumer.Commit();
                consumer.Close();
            }
        }
    }
}