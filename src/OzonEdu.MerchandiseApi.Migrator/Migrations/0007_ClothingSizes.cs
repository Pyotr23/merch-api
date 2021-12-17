using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(7)]
    public class ClothingSizes: Migration
    {
        public override void Up()
        {
            Create
                .Table("clothing_sizes")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("clothing_sizes");
        }
    }
}