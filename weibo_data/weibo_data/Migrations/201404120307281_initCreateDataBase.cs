namespace weibo_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initCreateDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WeiboFromBigDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentWeiboId = c.String(),
                        ForwardWeiboId = c.String(),
                        CommentCount = c.String(),
                        Content = c.String(),
                        CreateTime = c.String(),
                        PraiseCount = c.String(),
                        ReportCount = c.String(),
                        Source = c.String(),
                        UserId = c.String(),
                        WeiboId = c.String(),
                        WeiboUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeiboFromBigDatas");
        }
    }
}
