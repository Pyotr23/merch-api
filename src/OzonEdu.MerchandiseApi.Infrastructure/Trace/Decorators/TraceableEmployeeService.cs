using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Services;
using OzonEdu.MerchandiseApi.Infrastructure.Trace.Tracer;

namespace OzonEdu.MerchandiseApi.Infrastructure.Trace.Decorators
{
    public class TraceableEmployeeService : IEmployeeService, ITraceable
    {
        private const string ServiceName = nameof(TraceableEmployeeService);

        private readonly IEmployeeService _employeeService;

        public ICustomTracer Tracer { get; }

        public TraceableEmployeeService(IEmployeeService employeeService, ICustomTracer customTracer)
        {
            Tracer = customTracer;
            _employeeService = employeeService;
        }
        
        public async Task<Employee?> FindAsync(int id, CancellationToken token = default)
        {
            using var span = Tracer.GetSpan(ServiceName,
                nameof(FindAsync), 
                ("filter", "id"));
            return await _employeeService.FindAsync(id, token);
        }

        public async Task<Employee?> FindAsync(string email, CancellationToken token = default)
        {
            using var span = Tracer.GetSpan(ServiceName,
                nameof(FindAsync), 
                ("filter", "email"));
            return await _employeeService.FindAsync(email, token);
        }

        public async Task AddMerchDelivery(int employeeId, int merchDeliveryId, CancellationToken token)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(AddMerchDelivery));
            await _employeeService.AddMerchDelivery(employeeId, merchDeliveryId, token);
        }

        public async Task<Employee> CreateAsync(string name, string email, CancellationToken token = default)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(CreateAsync));
            return await _employeeService.CreateAsync(name, email, token);
        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken token = default)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(UpdateAsync));
            return await _employeeService.UpdateAsync(employee, token);
        }

        public async Task<IEnumerable<Employee>> GetAsync(MerchDeliveryStatus status, 
            IEnumerable<long> skuCollection, 
            CancellationToken token)
        {
            using var span = Tracer.GetSpan(ServiceName, nameof(GetAsync));
            return await _employeeService.GetAsync(status, skuCollection, token);
        }
    }
}