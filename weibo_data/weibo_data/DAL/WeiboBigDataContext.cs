using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using weibo_data.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace weibo_data.DAL
{
    
    public class WeiboBigDataContext:DbContext
    {
        public WeiboBigDataContext()
            : base("name=WeiboBigData")
        {

        }
        public DbSet<WeiboFromBigData> WeiboBigDatas { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}