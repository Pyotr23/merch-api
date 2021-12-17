namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record GetMerchDeliveryStatusRequest
    {
        public int? EmployeeId { get; set; } 
        public int? MerchPackTypeId { get; set; }
    }
}