using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class MerchDeliveryTests
    {
        [Fact]
        public void SetSkuCollection_NegativeSkuId_NegativeValueException()
        {
            var skuCollection = new Sku[]{new (-1)};

            Assert.Throws<NegativeValueException>(()
                => new MerchDelivery(MerchPackType.WelcomePack, skuCollection, MerchDeliveryStatus.Notify));
        }
        
        [Fact]
        public void SetSkuCollection_CorrectSkuCollection_Success()
        {
            var skuCollection = new List<Sku>{new (1), new (2)};

            var merchDelivery = new MerchDelivery(MerchPackType.WelcomePack, skuCollection, MerchDeliveryStatus.Notify);
            
            Assert.All(merchDelivery.SkuCollection, sku => skuCollection.Contains(sku));
        }
    }
}