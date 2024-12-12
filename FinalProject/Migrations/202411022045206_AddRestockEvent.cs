namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRestockEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Restocks",
                c => new
                    {
                        RestockID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        RestockDate = c.DateTime(nullable: false),
                        QuantityAdded = c.Int(nullable: false),
                        Summary = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.RestockID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Restocks", "ProductID", "dbo.Products");
            DropIndex("dbo.Restocks", new[] { "ProductID" });
            DropTable("dbo.Restocks");
        }
    }
}
