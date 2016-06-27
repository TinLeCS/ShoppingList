namespace ShoppingListTeam3.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ShoppingListAccount_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ShoppingListAccount", t => t.ShoppingListAccount_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ShoppingListAccount_Id);
            
            CreateTable(
                "dbo.ShoppingList",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Guid(nullable: false),
                        Name = c.String(),
                        Color = c.String(),
                        Group = c.String(),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifieddUtc = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ShoppingListID = c.Int(nullable: false),
                        Content = c.String(),
                        Priority = c.Int(nullable: false),
                        IsChecked = c.Boolean(nullable: false),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ShoppingList", t => t.ShoppingListID, cascadeDelete: true)
                .Index(t => t.ShoppingListID);
            
            CreateTable(
                "dbo.ShoppingListAccount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ShoppingListAccount_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShoppingListAccount", t => t.ShoppingListAccount_Id)
                .Index(t => t.ShoppingListAccount_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ShoppingListAccount_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ShoppingListAccount", t => t.ShoppingListAccount_Id)
                .Index(t => t.ShoppingListAccount_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "ShoppingListAccount_Id", "dbo.ShoppingListAccount");
            DropForeignKey("dbo.IdentityUserLogin", "ShoppingListAccount_Id", "dbo.ShoppingListAccount");
            DropForeignKey("dbo.IdentityUserClaim", "ShoppingListAccount_Id", "dbo.ShoppingListAccount");
            DropForeignKey("dbo.Item", "ShoppingListID", "dbo.ShoppingList");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropIndex("dbo.IdentityUserLogin", new[] { "ShoppingListAccount_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ShoppingListAccount_Id" });
            DropIndex("dbo.Item", new[] { "ShoppingListID" });
            DropIndex("dbo.IdentityUserRole", new[] { "ShoppingListAccount_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ShoppingListAccount");
            DropTable("dbo.Item");
            DropTable("dbo.ShoppingList");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
        }
    }
}
