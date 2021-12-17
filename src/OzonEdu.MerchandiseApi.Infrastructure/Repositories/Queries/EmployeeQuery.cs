namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Queries
{
    internal static class EmployeeQuery
    {
        internal const string Insert = @"
            INSERT INTO employees (name, clothing_size_id, email_address, manager_email_address)
            VALUES (@Name, @ClothingSizeId, @EmailAddress, @ManagerEmailAddress)
            RETURNING employee_id;";
        
        internal const string Update = @"
            UPDATE employees
            SET name = @Name, clothing_size_id = @ClothingSizeId, 
                email_address = @EmailAddress, manager_email_address = @ManagerEmailAddress
            WHERE employee_id = @EmployeeId;";
        
        internal const string FilterById = @"
            SELECT e.employee_id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                   cs.id, cs.name
            FROM employees e
            LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.id                
            WHERE e.employee_id = @Id";
        
        internal const string FilterByEmail = @"
            SELECT e.employee_id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                   cs.id, cs.name
            FROM employees e
            LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.clothing_size_id                
            WHERE e.email_address = @Email";
        
        internal const string FilterByMerchDeliveryStatusAndSkuCollection = @"
            SELECT e.employee_id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                   cs.id, cs.name
            FROM employees e
            INNER JOIN employee_merch_delivery_maps emdm ON e.employee_id = emdm.employee_id
            INNER JOIN merch_deliveries md ON emdm.merch_delivery_id = md.id            
            LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.clothing_size_id                
            WHERE md.merch_delivery_status_id = @StatusId
            AND md.sku_ids && @SkuIds;";

        internal const string AddMerchDelivery = @"
            INSERT INTO employee_merch_delivery_maps(employee_id, merch_delivery_id)
            VALUES (@EmployeeId, @MerchDeliveryId)
        ";
    }
}