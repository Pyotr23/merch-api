using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(11)]
    public class AddTestData : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO employees(name, clothing_size_id, email_address, manager_email_address)
                VALUES 
                    ('Petya', 5, 'pokolesnikov@gmail.com', null),
                    ('Tanya', 4, null, '2chilavert3@mail.ru')                    
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_deliveries(merch_delivery_status_id, merch_pack_type_id, status_change_date, sku_ids)
                VALUES 
                    (2, 10, now()::date, null),
                    (1, 50, now()::date, null)                    
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO employee_merch_delivery_maps(employee_id, merch_delivery_id)
                VALUES 
                    (1, 1),
                    (1, 2)                    
                ON CONFLICT DO NOTHING
            ");
        }
    }
}