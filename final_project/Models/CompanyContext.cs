using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class CompanyContext : DbContext
    {
        public DbSet<COMPANY> Companies { get; set; }
        public DbSet<COMPANYFEATURETAB> CompanyFeatureTabs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<COMPANYFEATURETAB>().HasRequired<COMPANY>(s => s.COMPANY).WithMany(s => s.CompanyFeatureTabs).HasForeignKey(s => s.CompanyID);
        }
    }
}