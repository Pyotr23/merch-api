using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors
{
    internal interface IAppealProcessor
    {
        MerchDeliveryStatus MerchDeliveryStatus { get; }
        Task Do(IEnumerable<long> skuCollection, CancellationToken token);
        Task TryCompleteDeliveries(Employee employee, CancellationToken token);
    }
}