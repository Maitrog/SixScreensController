using System.Collections.Generic;

namespace Six_Screens_Controller
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PlaylistElement> PlaylistElements { get; set; } = new List<PlaylistElement>();
    }
}
