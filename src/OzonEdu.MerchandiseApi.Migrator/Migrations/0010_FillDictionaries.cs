using FluentMigrator;

namespace OzonEdu.MerchandiseApi.Migrator.Migrations
{
    [Migration(10)]
    public class FillDictionaries : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO clothing_sizes (id, name)
                VALUES 
                    (1, 'XS'),
                    (2, 'S'),
                    (3, 'M'),
                    (4, 'L'),
                    (5, 'XL'),
                    (6, 'XXL')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_delivery_statuses (name)
                VALUES 
                    ('EmployeeCame'),
                    ('Done'),
                    ('Notify'),
                    ('InWork'),
                    ('NotIssued')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_types (name)
                VALUES 
                    ('TShirt'),
                    ('Sweatshirt'),
                    ('Notepad'),
                    ('Bag'),
                    ('Pen'),
                    ('Socks')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_pack_types (id, name, merch_type_ids)
                VALUES 
                    (10, 'WelcomePack', '{ 5 }'),
                    (20, 'ProbationPeriodEndingPack', '{ 6 }'),
                    (30, 'ConferenceListenerPack', '{ 3 }'),
                    (40, 'ConferenceSpeakerPack', '{ 1 }'),
                    (50, 'VeteranPack', '{ 2,4 }')
                ON CONFLICT DO NOTHING
            ");
        }
    }
}