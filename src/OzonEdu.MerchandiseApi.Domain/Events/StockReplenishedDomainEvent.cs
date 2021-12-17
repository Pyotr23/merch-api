using System.Collections.Generic;
using CSharpCourse.Core.Lib.Events;
using CSharpCourse.Core.Lib.Models;
using MediatR;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class StockReplenishedDomainEvent : INotification
    {
        public IReadOnlyCollection<StockReplenishedItem> Items { get; }

        public StockReplenishedDomainEvent(StockReplenishedEvent stockReplenishedEvent)
        {
            Items = stockReplenishedEvent.Type;
        }
    }
}