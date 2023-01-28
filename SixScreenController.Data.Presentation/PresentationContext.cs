using Microsoft.EntityFrameworkCore;
using SixScreensController.Data.Presentation.Entities;

namespace SixScreensController.Data.Presentation
{
    public class PresentationContext : DbContext
    {
        public DbSet<PresentationInfo> Presentations { get; set; }

        public PresentationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=Presentations.db");
        }
    }
}
