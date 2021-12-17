using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(6)]
    public class MerchDeliveries : Migration 
    {
        private const string TableName = "merch_deliveries";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("merch_delivery_id").AsInt32().Identity().PrimaryKey()
                .WithColumn("merch_delivery_status_id").AsInt32().NotNullable()
                .WithColumn("merch_pack_type_id").AsInt32().NotNullable()
                .WithColumn("status_change_date").AsDate().NotNullable()
                .WithColumn("sku_ids").AsCustom("bigint[]").Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}