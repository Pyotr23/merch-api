using System;

namespace OzonEdu.MerchandiseApi.Domain.Exceptions.MerchDeliveryAggregate
{
    public class MerchDeliveryAlreadyDone : Exception
    {
        public MerchDeliveryAlreadyDone(string message) : base(message)
        {}
        
        public MerchDeliveryAlreadyDone(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}