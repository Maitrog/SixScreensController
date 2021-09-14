using System.Collections.Generic;

namespace SixScreenControllerApi.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PlaylistElement> PlaylistElements { get; set; } = new List<PlaylistElement>();
    }
}
