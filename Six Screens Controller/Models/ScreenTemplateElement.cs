namespace Six_Screens_Controller.Models
{
    /// <summary>
    /// Template for one screen
    /// </summary>
    public class ScreenTemplateElement
    {
        /// <summary>
        /// Primary key in database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// File path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Is playlist or not
        /// </summary>
        public bool IsPlaylist { get; set; } = false;
        /// <summary>
        /// Numeber of screen
        /// </summary>
        public int ScreenNumber { get; set; }

        /// <summary>
        /// Foreign key in database
        /// </summary>
        public int ScreenTemplateId { get; set; }

        public override bool Equals(object obj)
        {
            if((obj as ScreenTemplateElement) != null)
                return (obj as ScreenTemplateElement).Id == Id && (obj as ScreenTemplateElement).Path == Path && (obj as ScreenTemplateElement).IsPlaylist == IsPlaylist;

            return false;
        }
    }
}
