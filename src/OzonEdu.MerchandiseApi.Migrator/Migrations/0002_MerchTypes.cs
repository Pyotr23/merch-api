using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(2)]
    public class MerchTypes : Migration
    {
        private const string TableName = "merch_types";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}