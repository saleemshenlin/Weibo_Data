using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WeiboDataWithSDK.Models;

namespace weibo_data.Dals
{
    /// <summary>
    /// 迁移数据库
    /// Enable-Migrations -ContextTypeName weibo_data.Dals.WeiboBigDataContext
    /// add-migration initDataBase
    /// update-database
    /// </summary>
    public class WeiboBigDataContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public WeiboBigDataContext()
            : base("name=DefaultConnection")
        {
        }
        public DbSet<User> UserDb { get; set; }
        public DbSet<Status> StatusDb { get; set; }
        public DbSet<Hazards> HarzardsDb { get; set; }
        public DbSet<StatusL> StatusLDb { get; set; }
        public DbSet<UserL> UserLDb { get; set; }
        //public System.Data.Entity.DbSet<Status> WeiboFromBigDatas { get; set; }

        //Enable-Migrations: Enables Code First Migrations in a project.
        //Add-Migration: Scaffolds a migration script for any pending model changes.
        //Update-Database: Applies any pending migrations to the database.
        //Get-Migrations: Displays the migrations that have been applied to the target database.

    }
}
