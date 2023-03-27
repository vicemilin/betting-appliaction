using System;
using FluentMigrator;

namespace VM.BettingApplication.Migrations.Migrations
{
    [Migration(202303121100)]
    public class Migration_20230312_createTicketTables : Migration
    {

        public override void Up()
        {
            Create.Table("ticket").InSchema("sport")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("payin").AsDecimal()
                .WithColumn("win").AsDecimal().Nullable()
                .WithColumn("status").AsInt16();

            Create.Table("ticket_bet").InSchema("sport")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("ticket_id").AsGuid().ForeignKey("fk_ticket_bet_ticket", "sport", "ticket", "id")
                .WithColumn("odds").AsDecimal()
                .WithColumn("status").AsInt16();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303121200)]
    public class Migration_20230312_createSportTable : Migration
    {

        public override void Up()
        {
            Create.Table("sport").InSchema("sport")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString()
                .WithColumn("available_picks").AsCustom("jsonb");

            Insert.IntoTable("sport").InSchema("sport")
                .Row(new
                {
                    id = Guid.Parse("b9738d19-8f75-4e91-b17e-9d2d2e4a6ba6"),
                    name = "Soccer",
                    available_picks =
                        System.Text.Json.JsonSerializer.Serialize(new string[] { "1", "x", "2", "1x", "x2", "12"})
                });


            Insert.IntoTable("sport").InSchema("sport")
                .Row(new
                {
                    id = Guid.Parse("d287c119-73ca-46f7-b172-582275e2ecf2"),
                    name = "Basketball",
                    available_picks =
                        System.Text.Json.JsonSerializer.Serialize(new string[] { "1", "x", "2", "1x", "x2", "12" })
                });

            Insert.IntoTable("sport").InSchema("sport")
                .Row(new
                {
                    id = Guid.Parse("6b6fc558-e23e-4be4-a5ec-ca3e048a597a"),
                    name = "Tennis",
                    available_picks =
                        System.Text.Json.JsonSerializer.Serialize(new string[] { "1", "2" })
                });
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303121300)]
    public class Migration_20230312_createEventsTables : Migration
    {

        public override void Up()
        {
            Create.Table("event").InSchema("sport")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("home_team").AsString()
                .WithColumn("away_team").AsString()
                .WithColumn("sport_id").AsGuid().ForeignKey("fk_event_sport", "sport", "sport", "id")
                .WithColumn("start_time").AsDateTime();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303122000)]
    public class Migration_20230312_createPicksTable : Migration
    {

        public override void Up()
        {
            Create.Table("pick").InSchema("sport")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("pick_name").AsString()
                .WithColumn("event_id").AsGuid().ForeignKey("fk_pick_event", "sport", "event", "id")
                .WithColumn("odds").AsDecimal()
                .WithColumn("status").AsInt16();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303161700)]
    public class Migration_20230312_addLinkedIdToEventTable : Migration
    {

        public override void Up()
        {
            Alter.Table("event").InSchema("sport")
                .AddColumn("is_top_offer").AsBoolean().WithDefaultValue(false)
                .AddColumn("linked_id").AsGuid().Nullable().Unique("uc_event_linked_id");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303161800)]
    public class Migration_20230312_addPayinTimeToTicketTable : Migration
    {

        public override void Up()
        {
            Alter.Table("ticket").InSchema("sport")
                .AddColumn("payin_time").AsDateTime();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303251800)]
    public class Migration_20230325_addPickToTicketBetTable : Migration
    {

        public override void Up()
        {
            Alter.Table("ticket_bet").InSchema("sport")
                .AddColumn("pick_id")
                .AsGuid().ForeignKey("fk_ticket_bet_pick", "sport", "pick", "id");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(202303271200)]
    public class Migration_20230327_AddTransactionTable : Migration
    {

        public override void Up()
        {
            Create.Table("wallet_transaction").InSchema("sport")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("type").AsInt16()
                .WithColumn("amount").AsDecimal()
                .WithColumn("transaction_date_time").AsDateTime()
                .WithColumn("current_state").AsDecimal();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}

