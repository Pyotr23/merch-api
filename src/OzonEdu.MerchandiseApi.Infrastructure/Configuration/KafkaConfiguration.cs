using System.ComponentModel.DataAnnotations;

namespace OzonEdu.MerchandiseApi.Infrastructure.Configuration
{
    public class KafkaConfiguration
    {
        [Required]
        public string? BootstrapServers { get; set; }
        
        [Required]
        public string? StockReplenishedEventTopic { get; set; }
        
        [Required]
        public string? EmployeeNotificationEventTopic { get; set; }
        
        [Required]
        public string? GroupId { get; set; }
    }
}