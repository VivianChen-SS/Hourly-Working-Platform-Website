using final_project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace final_project.DAL
{
    public class AllTablesContext : DbContext
    {
        public DbSet<EMPLOYER> Employers { get; set; }
    }
}