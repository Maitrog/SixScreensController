using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Six_Screens_Controller
{
    class TemplateContext : DbContext
    {
        public DbSet<ScreenTemplate> ScreenTemplates { get; set; }
        public DbSet<ScreenTemplateElement> ScreenTemplateElements { get; set; } 

        public TemplateContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=VkScreenTemplate.db");
        }
    }
}
