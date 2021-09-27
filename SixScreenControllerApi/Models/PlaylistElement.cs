namespace SixScreenControllerApi.Models
{
    /// <summary>
    /// Represent element for playlist
    /// </summary>
    public class PlaylistElement
    {
        /// <summary>
        /// Primary kay in database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Path to file
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Element's duration
        /// </summary>
        public int Duration { get; set; } = 10;

        /// <summary>
        /// Foreign key in database
        /// </summary>
        public int PlaylistId { get; set; }
    }
}
