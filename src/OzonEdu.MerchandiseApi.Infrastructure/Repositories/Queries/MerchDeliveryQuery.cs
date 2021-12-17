namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Queries
{
    internal static class MerchDeliveryQuery
    {
        internal const string Insert = @"
            INSERT INTO merch_deliveries (merch_delivery_status_id, merch_pack_type_id, status_change_date, sku_ids)
            VALUES (@MerchDeliveryStatusId, @MerchPackTypeId, @StatusChangeDate, @SkuIds)
            RETURNING merch_delivery_id;";
        
        internal const string Update = @"
            UPDATE merch_deliveries 
            SET merch_delivery_status_id = @MerchDeliveryStatusId, 
                merch_pack_type_id = @MerchPackTypeId, 
                status_change_date = @StatusChangeDate,
                sku_ids = @SkuIds
            WHERE merch_delivery_id = @MerchDeliveryId;";
        
        internal const string FilterByEmployeeId = @"
            SELECT md.merch_delivery_id, md.merch_pack_type_id, md.merch_delivery_status_id, md.status_change_date, md.sku_ids,
                mpt.id, mpt.name, mpt.merch_type_ids, mds.id, mds.name
            FROM merch_deliveries md
            INNER JOIN employee_merch_delivery_maps emdm ON md.merch_delivery_id = emdm.merch_delivery_id
            LEFT JOIN merch_pack_types mpt ON md.merch_pack_type_id = mpt.id
            LEFT JOIN merch_delivery_statuses mds ON md.merch_delivery_status_id = mds.id
            WHERE emdm.employee_id = @EmployeeId";
        
        internal const string FilterByEmployeeIdAndStatusIdAndSkuCollection = @"
            SELECT md.merch_delivery_id, md.merch_pack_type_id, md.merch_delivery_status_id, md.status_change_date, md.sku_ids,
                mpt.id, mpt.name, mpt.merch_type_ids, mds.id, mds.name
            FROM merch_deliveries md
            INNER JOIN employee_merch_delivery_maps emdm ON md.merch_delivery_id = emdm.merch_delivery_id
            LEFT JOIN merch_pack_types mpt ON md.merch_pack_type_id = mpt.id
            LEFT JOIN merch_delivery_statuses mds ON md.merch_delivery_status_id = mds.id
            WHERE emdm.employee_id = @EmployeeId
            AND md.merch_delivery_status_id = @MerchDeliveryStatusId
            AND md.sku_ids && @SkuCollection;";
        
        internal const string GetMerchTypes = @"
            SELECT mt.id, mt.name
            FROM merch_types mt;";
        
        internal const string FindMerchDeliveryStatusByEmployeeIdAndMerchPackTypeId = @"
            SELECT mds.id, mds.name
            FROM merch_deliveries md
            INNER JOIN employee_merch_delivery_maps emdm ON md.merch_delivery_id = emdm.merch_delivery_id
            LEFT JOIN merch_pack_types mpt ON md.merch_pack_type_id = mpt.id
            LEFT JOIN merch_delivery_statuses mds ON md.merch_delivery_status_id = mds.id
            WHERE emdm.employee_id = @EmployeeId
            AND mpt.id = @MerchPackTypeId";
        
        internal const string FindMerchPackType = @"
            SELECT mpt.id, mpt.name
            FROM merch_pack_types mpt             
            WHERE mpt.id = @MerchPackTypeId";
    }
}