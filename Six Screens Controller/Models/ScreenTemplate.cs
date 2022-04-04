using System.Collections.Generic;

namespace Six_Screens_Controller.Models
{
    /// <summary>
    /// Template for screens
    /// </summary>
    public class ScreenTemplate
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Template title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// List of elements in template. See also <seealso cref="ScreenTemplateElement"/>
        /// </summary>
        public List<ScreenTemplateElement> ScreenTemplateElements { get; set; } = new List<ScreenTemplateElement>(6);

        public ScreenTemplate()
        {
            Id = 0;
            Title = string.Empty;
            ScreenTemplateElements = new List<ScreenTemplateElement>(6);
        }

        public ScreenTemplate(string filePath)
        {
            Id = 0;
            Title = string.Empty;
            ScreenTemplateElements = new List<ScreenTemplateElement>
            {
                new ScreenTemplateElement() { Path = filePath, IsPlaylist = false, ScreenNumber = 1 },
                new ScreenTemplateElement() { Path = filePath, IsPlaylist = false, ScreenNumber = 2 },
                new ScreenTemplateElement() { Path = filePath, IsPlaylist = false, ScreenNumber = 3 },
                new ScreenTemplateElement() { Path = filePath, IsPlaylist = false, ScreenNumber = 4 },
                new ScreenTemplateElement() { Path = filePath, IsPlaylist = false, ScreenNumber = 5 },
                new ScreenTemplateElement() { Path = filePath, IsPlaylist = false, ScreenNumber = 6 },
            };
        }
    }
}
