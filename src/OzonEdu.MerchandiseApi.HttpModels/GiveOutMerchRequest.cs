namespace OzonEdu.MerchandiseApi.HttpModels
{
    public record GiveOutMerchRequest
    {
        public int? EmployeeId { get; set; } 
        public int? MerchPackTypeId { get; set; }
    }
}