using System.Collections.Generic;

namespace Six_Screens_Controller
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
    }
}
