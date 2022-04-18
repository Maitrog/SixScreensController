using Microsoft.EntityFrameworkCore;

namespace Six_Screens_Controller.Models
{
    internal class PresentationContext: DbContext
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
