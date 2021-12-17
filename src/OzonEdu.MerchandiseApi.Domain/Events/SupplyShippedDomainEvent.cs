using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class SupplyShippedDomainEvent : INotification
    {
        public SupplyShippedEvent SupplyShippedEvent { get; }

        public SupplyShippedDomainEvent(SupplyShippedEvent supplyShippedEvent)
        {
            SupplyShippedEvent = supplyShippedEvent;
        }
    }
}