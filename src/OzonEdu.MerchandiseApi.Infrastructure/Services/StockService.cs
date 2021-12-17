using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchandiseApi.Infrastructure.Services
{
    public class StockService : IStockService
    {
        private readonly StockApiGrpc.StockApiGrpcClient _stockClient;

        public StockService(StockApiGrpc.StockApiGrpcClient stockClient)
        {
            _stockClient = stockClient;
        }
        
        public async Task<bool> IsReadyToGiveOut(MerchDelivery delivery, CancellationToken token)
        {
            var request = new SkusRequest();
            request.Skus.AddRange(delivery
                .SkuCollection
                .Select(s => s.Value));
                
            var response = await _stockClient
                .GetStockItemsAvailabilityAsync(request, cancellationToken: token);
                
            return response
                .Items
                .All(i => i.Quantity > 0);
        }

        public async Task<bool> TryGiveOutItems(MerchDelivery delivery, CancellationToken token)
        {
            var request = new GiveOutItemsRequest();
            
            var skuQuantityItems = delivery
                .SkuCollection
                .Select(s => new SkuQuantityItem
                {
                    Sku = s.Value,
                    Quantity = 1
                });
            
            request
                .Items
                .AddRange(skuQuantityItems);

            var response = await _stockClient.GiveOutItemsAsync(request, cancellationToken: token);

            return response.Result == GiveOutItemsResponse.Types.Result.Successful;
        }
    }
}