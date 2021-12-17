using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Models
{
    public class MerchTypeEnumeration : Enumeration
    {
        public List<MerchType> MerchTypes { get; } = new();
        
        public MerchTypeEnumeration(int id, string name, IEnumerable<MerchType> merchTypes) : base(id, name)
        {
            MerchTypes.AddRange(merchTypes);
        }
    }
}