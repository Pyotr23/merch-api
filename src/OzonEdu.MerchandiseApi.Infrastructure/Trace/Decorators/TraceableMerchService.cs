using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Services;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;

namespace OzonEdu.MerchandiseApi.Infrastructure.Trace.Decorators
{
    public class TraceableMerchService : IMerchService, ITraceable
    {
        private const string ServiceName = nameof(TraceableMerchService);

        private readonly IMerchService _merchService;

        public ICustomTracer Tracer { get; }

        public TraceableMerchService(IMerchService merchService, ICustomTracer tracer)
        {
            _merchService = merchService;
            Tracer = tracer;
        }
        
        public async Task<MerchDelivery> CreateMerchDeliveryAsync(MerchPackType merchPackType, ClothingSize? size, CancellationToken token)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(CreateMerchDeliveryAsync));
            return await _merchService.CreateMerchDeliveryAsync(merchPackType, size, token);
        }

        public async Task<MerchDeliveryStatus?> FindStatus(int employeeId, int merchPackTypeId, CancellationToken token)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(FindStatus));
            return await _merchService.FindStatus(employeeId, merchPackTypeId, token);
        }

        public async Task<MerchPackType?> FindMerchPackType(int typeId, CancellationToken token)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(FindMerchPackType));
            return await _merchService.FindMerchPackType(typeId, token);
        }

        public async Task<MerchDelivery?> UpdateAsync(MerchDelivery delivery, CancellationToken token)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(UpdateAsync));
            return await _merchService.UpdateAsync(delivery, token);        
        }
    }
}