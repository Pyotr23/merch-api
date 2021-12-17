using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class StatusChangeDate : ValueObject
    {
        public DateTime Value { get; }

        public StatusChangeDate(DateTime date)
        {
            Value = date;
        }
        
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}