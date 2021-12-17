using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class Sku : ValueObject
    {
        public long Value { get; }
        
        public Sku(long sku)
        {
            Value = sku;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}