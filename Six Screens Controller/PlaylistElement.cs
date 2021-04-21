using System;
using System.Collections.Generic;
using System.Text;

namespace Six_Screens_Controller
{
    public class PlaylistElement
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int Duration { get; set; } = 10000;

        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
