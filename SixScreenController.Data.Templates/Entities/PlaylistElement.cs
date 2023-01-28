namespace SixScreenController.Data.Templates.Entities
{
    /// <summary>
    /// Represent element for playlist.
    /// </summary>
    public class PlaylistElement
    {
        /// <summary>
        /// Gets or sets primary kay in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets path to file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets element's duration.
        /// </summary>
        public int Duration { get; set; } = 10;

        /// <summary>
        /// Gets or sets foreign key in database.
        /// </summary>
        public int PlaylistId { get; set; }
    }
}
