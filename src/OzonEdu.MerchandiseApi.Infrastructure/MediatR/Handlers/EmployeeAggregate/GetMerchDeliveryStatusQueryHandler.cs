using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Exceptions;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Queries;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.EmployeeAggregate
{
    public class GetMerchDeliveryStatusQueryHandler : IRequestHandler<GetMerchDeliveryStatusQuery, string>
    {
        private readonly IMerchService _merchService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomTracer _tracer;

        public GetMerchDeliveryStatusQueryHandler(IMerchService merchService, 
            IEmployeeService employeeService,
            ICustomTracer tracer)
        {
            _employeeService = employeeService;
            _merchService = merchService;
            _tracer = tracer;
        }
        
        public async Task<string> Handle(GetMerchDeliveryStatusQuery request, CancellationToken token)
        {
            using var span = _tracer.GetSpan(nameof(GetMerchDeliveryStatusQueryHandler), nameof(Handle));
            
            var employee = await _employeeService.FindAsync(request.EmployeeId, token);
            if (employee is null)
                throw new NotExistsException($"Employee with id={request.EmployeeId} does not exists");

            var merchPackType = await _merchService.FindMerchPackType(request.MerchPackTypeId, token);
            if (merchPackType is null)
            {
                throw new NotExistsException(
                    $"Merch pack type with id={request.MerchPackTypeId} does not exists");
            }
            
            var status = await _merchService
                .FindStatus(request.EmployeeId, request.MerchPackTypeId, token);
            
            return status?.Name ?? MerchDeliveryStatus.NotIssued.Name;
        }
    }
}