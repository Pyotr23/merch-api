using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(8)]
    public class Employees : Migration
    {
        private const string TableName = "employees";
        
        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("employee_id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("clothing_size_id").AsInt32().Nullable()
                .WithColumn("email_address").AsString().Nullable()
                .WithColumn("manager_email_address").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}