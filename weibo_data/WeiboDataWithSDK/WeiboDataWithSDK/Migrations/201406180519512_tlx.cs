namespace WeiboDataWithSDK.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tlx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusLs",
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
                .ForeignKey("dbo.UserLs", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UserLs",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDStr = c.String(),
                        ScreenName = c.String(),
                        Name = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Location = c.String(),
                        Description = c.String(),
                        ProfileImageUrl = c.String(),
                        Gender = c.String(),
                        CreatedAt = c.String(),
                        VerifiedType = c.String(),
                        Remark = c.String(),
                        AllowAllComment = c.Boolean(nullable: false),
                        AvatarLarge = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StatusLs", "UserID", "dbo.UserLs");
            DropIndex("dbo.StatusLs", new[] { "UserID" });
            DropTable("dbo.UserLs");
            DropTable("dbo.StatusLs");
        }
    }
}
