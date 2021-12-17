namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public record MerchDeliveryStatus
    {
        public int? Id { get; init; }
        public string? Name { get; init; }
    }
}