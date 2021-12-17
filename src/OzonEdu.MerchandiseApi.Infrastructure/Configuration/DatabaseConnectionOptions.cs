using System.ComponentModel.DataAnnotations;

namespace OzonEdu.MerchandiseApi.Infrastructure.Configuration
{
    public class DatabaseConnectionOptions
    {
        [Required]
        public string? ConnectionString { get; set; }
    }
}