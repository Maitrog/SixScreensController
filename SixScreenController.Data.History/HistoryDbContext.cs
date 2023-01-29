using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SixScreenController.Data.Templates;
using SixScreenController.Data.Templates.Entities;
using THistory = SixScreenController.Data.History.Entities.History;

namespace SixScreenController.Data.History
{
    public class HistoryDbContext : DbContext
    {
        public DbSet<THistory> History { get; set; }

        public DbSet<ScreenTemplate> ScreenTemplates { get; set; }

        public DbSet<ScreenTemplateElement> ScreenTemplateElements { get; set; }

        public HistoryDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=History.db");
        }
    }
}
