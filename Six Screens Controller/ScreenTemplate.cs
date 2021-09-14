using System.Collections.Generic;

namespace Six_Screens_Controller
{
    public class ScreenTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ScreenTemplateElement> ScreenTemplateElements { get; set; } = new List<ScreenTemplateElement>(6);
    }
}
