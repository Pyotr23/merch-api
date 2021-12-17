using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(9)]
    public class EmployeeMerchDeliveryMaps : Migration
    {
        private const string TableName = "employee_merch_delivery_maps";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("employee_id").AsInt32().NotNullable()
                .WithColumn("merch_delivery_id").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}