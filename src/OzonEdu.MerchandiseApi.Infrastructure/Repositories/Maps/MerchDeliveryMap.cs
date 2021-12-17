using System;
using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Maps
{
    internal static class MerchDeliveryMap
    {
        internal static MerchDelivery CreateMerchDelivery(Models.MerchDelivery? deliveryModel,
            Models.MerchPackType? packTypeModel, 
            Models.MerchDeliveryStatus? statusModel,
            Dictionary<int, MerchType> merchTypes)
        {
            if (deliveryModel is null || packTypeModel is null || statusModel is null)
                return null;

            var packType = new MerchPackType(packTypeModel.Id,
                packTypeModel.Name,
                packTypeModel.MerchTypeIds?.Select(id => merchTypes[id]) 
                    ?? Array.Empty<MerchType>());

            var skuCollection = deliveryModel
                .SkuCollection?
                .Select(s => new Sku(s))
                .ToArray();

            var status = new MerchDeliveryStatus(statusModel.Id.Value, statusModel.Name);
            var statusChangeDate = new StatusChangeDate(deliveryModel.StatusChangeDate.Value);
            
            return new MerchDelivery(
                deliveryModel.MerchDeliveryId,
                packType,
                skuCollection,
                status,
                statusChangeDate);
        }
    }
}