namespace ShoppingListTeam3.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyShoppingListInDataLayer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShoppingList", "ModifieddUtc", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShoppingList", "ModifieddUtc", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
