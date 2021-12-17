using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.MessageBroker;
#pragma warning disable 8604

namespace OzonEdu.MerchandiseApi.HostedServices
{
    public class StockReplenishedHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockReplenishedHostedService> _logger;
        private readonly IMediator _mediator;
        private readonly KafkaManager _kafka;

        public StockReplenishedHostedService(IServiceScopeFactory scopeFactory,
            ILogger<StockReplenishedHostedService> logger,
            IMediator mediator,
            KafkaManager kafka)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
            _kafka = kafka;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = _kafka.Configuration.StockReplenishedEventTopic;
            await _kafka.StartConsuming(topic, _scopeFactory, PublishEvent, stoppingToken);
        }

        private async Task PublishEvent(string serializedMessage, CancellationToken token)
        {
            var message = JsonSerializer.Deserialize<StockReplenishedEvent>(serializedMessage);
            if (message is null)
                return;
            await _mediator.Publish(new StockReplenishedDomainEvent(message), token);
        }
    }
}