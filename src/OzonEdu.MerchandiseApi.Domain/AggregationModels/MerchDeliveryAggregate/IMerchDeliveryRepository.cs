using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public interface IMerchDeliveryRepository : IRepository<MerchDelivery>
    {
        Task<IEnumerable<MerchDelivery>?> GetAsync(int employeeId, CancellationToken token);

        Task<IEnumerable<MerchDelivery>> GetAsync(int employeeId,
            int statusId,
            IEnumerable<long> skuCollection,
            CancellationToken token);
        
        Task<MerchDeliveryStatus?> FindStatus(int employeeId, int merchPackTypeId, CancellationToken token);
        
        Task<MerchPackType?> FindMerchPackType(int typeId, CancellationToken token);
    }
}