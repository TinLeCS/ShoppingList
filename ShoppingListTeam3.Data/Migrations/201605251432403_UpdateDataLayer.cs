namespace ShoppingListTeam3.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataLayer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Item", "ShoppingList_ID", "dbo.ShoppingList");
            DropForeignKey("dbo.Item", "Note_ID", "dbo.Note");
            DropIndex("dbo.Item", new[] { "Note_ID" });
            DropIndex("dbo.Item", new[] { "ShoppingList_ID" });
            RenameColumn(table: "dbo.Item", name: "ShoppingList_ID", newName: "ShoppingListID");
            RenameColumn(table: "dbo.Item", name: "Note_ID", newName: "NoteID");
            AlterColumn("dbo.Item", "ModifiedUtc", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Item", "NoteID", c => c.Int(nullable: false));
            AlterColumn("dbo.Item", "ShoppingListID", c => c.Int(nullable: false));
            AlterColumn("dbo.Note", "ModifiedUtc", c => c.DateTimeOffset(precision: 7));
            CreateIndex("dbo.Item", "ShoppingListID");
            CreateIndex("dbo.Item", "NoteID");
            AddForeignKey("dbo.Item", "ShoppingListID", "dbo.ShoppingList", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Item", "NoteID", "dbo.Note", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "NoteID", "dbo.Note");
            DropForeignKey("dbo.Item", "ShoppingListID", "dbo.ShoppingList");
            DropIndex("dbo.Item", new[] { "NoteID" });
            DropIndex("dbo.Item", new[] { "ShoppingListID" });
            AlterColumn("dbo.Note", "ModifiedUtc", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Item", "ShoppingListID", c => c.Int());
            AlterColumn("dbo.Item", "NoteID", c => c.Int());
            AlterColumn("dbo.Item", "ModifiedUtc", c => c.DateTimeOffset(nullable: false, precision: 7));
            RenameColumn(table: "dbo.Item", name: "NoteID", newName: "Note_ID");
            RenameColumn(table: "dbo.Item", name: "ShoppingListID", newName: "ShoppingList_ID");
            CreateIndex("dbo.Item", "ShoppingList_ID");
            CreateIndex("dbo.Item", "Note_ID");
            AddForeignKey("dbo.Item", "Note_ID", "dbo.Note", "ID");
            AddForeignKey("dbo.Item", "ShoppingList_ID", "dbo.ShoppingList", "ID");
        }
    }
}
