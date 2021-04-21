using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Six_Screens_Controller
{
    class PlaylistContext: DbContext
    {
        public DbSet<PlaylistElement> PlaylistElements { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        public PlaylistContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=VkPlaylist.db");
        }
    }
}
