namespace WeiboDataWithSDK.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hazardsweibo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hazards",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.String(),
                        Text = c.String(),
                        Source = c.String(),
                        ThumbnailPictureUrl = c.String(),
                        MiddleSizePictureUrl = c.String(),
                        OriginalPictureUrl = c.String(),
                        RepostsCount = c.Int(nullable: false),
                        CommentsCount = c.Int(nullable: false),
                        AttitudeCount = c.Int(nullable: false),
                        Long = c.Single(nullable: false),
                        Lat = c.Single(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hazards", "UserID", "dbo.Users");
            DropIndex("dbo.Hazards", new[] { "UserID" });
            DropTable("dbo.Hazards");
        }
    }
}
