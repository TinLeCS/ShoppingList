namespace ShoppingListTeam3.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingDataLayer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Item", "NoteID", "dbo.Note");
            DropIndex("dbo.Item", new[] { "NoteID" });
            DropColumn("dbo.Item", "NoteID");
            DropTable("dbo.Note");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedUtc = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Item", "NoteID", c => c.Int(nullable: false));
            CreateIndex("dbo.Item", "NoteID");
            AddForeignKey("dbo.Item", "NoteID", "dbo.Note", "ID", cascadeDelete: true);
        }
    }
}
