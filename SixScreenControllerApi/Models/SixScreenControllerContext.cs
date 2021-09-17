using Microsoft.EntityFrameworkCore;

namespace SixScreenControllerApi.Models
{
    public class SixScreenControllerContext: DbContext
    {
        public DbSet<ScreenTemplate> ScreenTemplates { get; set; }
        public DbSet<ScreenTemplateElement> ScreenTemplateElements { get; set; }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistElement> PlaylistElements { get; set; }

        public SixScreenControllerContext(DbContextOptions<SixScreenControllerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
