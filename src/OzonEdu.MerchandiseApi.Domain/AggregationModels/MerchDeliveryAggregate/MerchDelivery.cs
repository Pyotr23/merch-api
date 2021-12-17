using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Exceptions;
using OzonEdu.MerchandiseApi.Domain.Exceptions.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class MerchDelivery : Entity
    {

        public MerchPackType MerchPackType { get; }

        public MerchDeliveryStatus Status { get; set; } = MerchDeliveryStatus.NotIssued;
        
        public ICollection<Sku> SkuCollection { get; } = new List<Sku>();
        
        public StatusChangeDate? StatusChangeDate { get; }

        public MerchDelivery(int id, 
            MerchPackType type, 
            IEnumerable<Sku> skuCollection, 
            MerchDeliveryStatus status,
            StatusChangeDate statusChangeDate)
            : this(type, skuCollection, status, statusChangeDate)
        {
            Id = id;
        }
        
        public MerchDelivery(MerchPackType merchPackType, 
            IEnumerable<Sku> skuCollection, 
            MerchDeliveryStatus status,
            StatusChangeDate statusChangeDate)
            : this(merchPackType, status)
        {
            SetSkuCollection(skuCollection);
            StatusChangeDate = statusChangeDate;
        }
        
        public MerchDelivery(MerchPackType merchPackType, IEnumerable<Sku>? skuCollection, MerchDeliveryStatus status)
            : this(merchPackType, status)
        {
            SetSkuCollection(skuCollection);
        }

        public MerchDelivery(MerchPackType merchPackType, MerchDeliveryStatus status)
        {
            MerchPackType = merchPackType;
            SetStatus(status);
        }
        
        public void SetStatus(MerchDeliveryStatus newStatus)
        { 
            if (Status.Equals(MerchDeliveryStatus.Done))
                throw new MerchDeliveryAlreadyDone($"The application (id={Id}) was completed");
            Status = newStatus; 
        }

        public void SetSkuCollection(IEnumerable<Sku>? skuCollection)
        {
            if (skuCollection is null)
                return;
            
            foreach (var sku in skuCollection)
            {
                if (sku.Value < 0)
                    throw new NegativeValueException("Sku value is less zero");
                
                SkuCollection.Add(sku);
            }
        }
    }
}