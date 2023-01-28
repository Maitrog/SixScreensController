using System.Collections.Generic;

namespace SixScreenController.Data.Templates.Entities
{
    /// <summary>
    /// Represents playlist 
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Gets or sets primary key in database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of playlist
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets list of elements in playlist. See also <seealso cref="PlaylistElement"/>
        /// </summary>
        public List<PlaylistElement> PlaylistElements { get; set; } = new List<PlaylistElement>();
    }
}
