using System;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public record MerchDelivery
    {
        public int MerchDeliveryId { get; init; }
        public DateTime? StatusChangeDate { get; init; }
        public long[]? SkuCollection { get; init; } 
    }
}