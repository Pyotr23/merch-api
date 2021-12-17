using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class MerchType : Enumeration
    {
        public static MerchType TShirt = new(1, nameof(TShirt));
        public static MerchType Sweatshirt = new(2, nameof(Sweatshirt));
        public static MerchType Notepad = new(3, nameof(Notepad));
        public static MerchType Bag = new(4, nameof(Bag));
        public static MerchType Pen = new(5, nameof(Pen));
        public static MerchType Socks = new(6, nameof(Socks));

        public MerchType(int id, string name) : base(id, name) 
        { }
    }
}