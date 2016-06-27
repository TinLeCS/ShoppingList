namespace ShoppingListTeam3.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Item", "ShoppingListID", "dbo.ShoppingList");
            DropIndex("dbo.Item", new[] { "ShoppingListID" });
            RenameColumn(table: "dbo.Item", name: "ShoppingListID", newName: "ShoppingList_ID");
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Item", "Note_ID", c => c.Int());
            AlterColumn("dbo.Item", "ShoppingList_ID", c => c.Int());
            CreateIndex("dbo.Item", "Note_ID");
            CreateIndex("dbo.Item", "ShoppingList_ID");
            AddForeignKey("dbo.Item", "Note_ID", "dbo.Note", "ID");
            AddForeignKey("dbo.Item", "ShoppingList_ID", "dbo.ShoppingList", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "ShoppingList_ID", "dbo.ShoppingList");
            DropForeignKey("dbo.Item", "Note_ID", "dbo.Note");
            DropIndex("dbo.Item", new[] { "ShoppingList_ID" });
            DropIndex("dbo.Item", new[] { "Note_ID" });
            AlterColumn("dbo.Item", "ShoppingList_ID", c => c.Int(nullable: false));
            DropColumn("dbo.Item", "Note_ID");
            DropTable("dbo.Note");
            RenameColumn(table: "dbo.Item", name: "ShoppingList_ID", newName: "ShoppingListID");
            CreateIndex("dbo.Item", "ShoppingListID");
            AddForeignKey("dbo.Item", "ShoppingListID", "dbo.ShoppingList", "ID", cascadeDelete: true);
        }
    }
}
