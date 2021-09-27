using System.Collections.Generic;

namespace SixScreenControllerApi.Models
{
    /// <summary>
    /// Represents playlist 
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Primary key in database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Title of playlist
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// List of elements in playlist. See also <seealso cref="PlaylistElement"/>
        /// </summary>
        public List<PlaylistElement> PlaylistElements { get; set; } = new List<PlaylistElement>();
    }
}
