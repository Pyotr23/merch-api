using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public interface IMerchHttpClient
    {
        Task GiveOutMerch(GiveOutMerchRequest request, CancellationToken token);
        
        Task<GetMerchDeliveryStatusResponse?> GetMerchDeliveryStatusRequest(
            GetMerchDeliveryStatusRequest request, CancellationToken token);
    }
}