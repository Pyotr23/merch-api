using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors;
using OzonEdu.MerchandiseApi.Infrastructure.Trace;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.DomainEvent
{
    public class StockReplenishedDomainEventHandler : INotificationHandler<StockReplenishedDomainEvent>
    {
        private readonly ILogger<StockReplenishedDomainEventHandler> _logger;
        private readonly AutoAppealProcessor _autoAppealProcessor;
        private readonly ManualAppealProcessor _manualAppealProcessor;
        private readonly ICustomTracer _tracer;

        public StockReplenishedDomainEventHandler(ILogger<StockReplenishedDomainEventHandler> logger,
            AutoAppealProcessor autoAppealProcessor,
            ManualAppealProcessor manualAppealProcessor,
            ICustomTracer tracer)
        {
            _tracer = tracer;
            _manualAppealProcessor = manualAppealProcessor;
            _autoAppealProcessor = autoAppealProcessor;
            _logger = logger;
        }
        
        public async Task Handle(StockReplenishedDomainEvent notification, CancellationToken token)
        {
            using var span = _tracer.GetSpan(nameof(StockReplenishedDomainEventHandler), nameof(Handle));
            var skuCollection = notification
                .Items
                .Select(it => it.Sku)
                .ToArray();

            await _manualAppealProcessor.Do(skuCollection, token);
            await _autoAppealProcessor.Do(skuCollection, token);
        }
    }
}