using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> FindAsync(int id, CancellationToken token = default);

        Task<Employee?> FindAsync(string email, CancellationToken token = default);

        Task AddMerchDelivery(int employeeId, int merchDeliveryId, CancellationToken token);
        
        Task<IEnumerable<Employee>> GetByMerchDeliveryStatusAndSkuCollection(int statusId, 
            IEnumerable<long> skuIdCollection,
            CancellationToken token = default);
    }
}