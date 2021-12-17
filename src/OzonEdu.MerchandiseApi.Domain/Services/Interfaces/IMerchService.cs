using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Services.Interfaces
{
    public interface IMerchService
    {
        Task<MerchDelivery> CreateMerchDeliveryAsync(MerchPackType merchPackType, 
            ClothingSize? size, 
            CancellationToken token);
        Task<MerchDeliveryStatus?> FindStatus(int employeeId, int merchPackTypeId, CancellationToken token);
        Task<MerchPackType?> FindMerchPackType(int typeId, CancellationToken token);
        Task<MerchDelivery?> UpdateAsync(MerchDelivery delivery, CancellationToken token);
    }
}