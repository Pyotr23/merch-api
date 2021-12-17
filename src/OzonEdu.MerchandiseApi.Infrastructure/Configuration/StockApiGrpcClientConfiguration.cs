using System.ComponentModel.DataAnnotations;

namespace OzonEdu.MerchandiseApi.Infrastructure.Configuration
{
    public class StockApiGrpcClientConfiguration
    {
        [Required, Url]
        public string? Address { get; set; }
    }
}