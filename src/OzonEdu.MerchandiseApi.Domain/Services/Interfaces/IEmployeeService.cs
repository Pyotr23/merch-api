using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee?> FindAsync(int id, CancellationToken token = default);
        
        Task<Employee?> FindAsync(string email, CancellationToken token = default);

        Task AddMerchDelivery(int employeeId, int merchDeliveryId, CancellationToken token);
        
        Task<Employee> CreateAsync(string name, string email, CancellationToken token = default);
        
        Task<Employee> UpdateAsync(Employee employee, CancellationToken token = default);
        
        Task<IEnumerable<Employee>> GetAsync(MerchDeliveryStatus status,
            IEnumerable<long> skuCollection,
            CancellationToken token = default);
    }
}