using System;
using System.Collections.Generic;
using System.Text;

namespace Six_Screens_Controller
{
    public class ScreenTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ScreenTemplateElement> ScreenTemplateElements { get; set; } = new List<ScreenTemplateElement>(6);
    }
}
