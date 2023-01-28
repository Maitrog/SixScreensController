using Microsoft.EntityFrameworkCore;
using SixScreenController.Data.Templates.Entities;

namespace SixScreenController.Data.Templates
{
    /// <summary>
    /// Database with paylists, playlist elements, screen templates, screen template elments
    /// </summary>
    public class SixScreenControllerContext : DbContext
    {
        public DbSet<ScreenTemplate> ScreenTemplates { get; set; }
        public DbSet<ScreenTemplateElement> ScreenTemplateElements { get; set; }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistElement> PlaylistElements { get; set; }

        public SixScreenControllerContext(DbContextOptions<SixScreenControllerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=VkScreenController.db");
        }
    }
}
