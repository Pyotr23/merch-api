using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.MessageBroker;
#pragma warning disable 8604

namespace OzonEdu.MerchandiseApi.HostedServices
{
    public class EmployeeNotificationHostedService : BackgroundService
    {
        private readonly KafkaManager _kafka;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMediator _mediator;

        public EmployeeNotificationHostedService(KafkaManager kafka,
            IServiceScopeFactory scopeFactory,
            IMediator mediator)
        {
            _mediator = mediator;
            _scopeFactory = scopeFactory;
            _kafka = kafka;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = _kafka.Configuration.EmployeeNotificationEventTopic;
            await _kafka.StartConsuming(topic, _scopeFactory, PublishEvent, stoppingToken);
        }

        private async Task PublishEvent(string serializedMessage, CancellationToken token)
        {
            var message = JsonSerializer.Deserialize<NotificationEvent>(serializedMessage);
            if (message is null)
                return;
            await _mediator.Publish(new EmployeeNotificationDomainEvent(message), token);
        }
    }
}