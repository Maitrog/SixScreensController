using System.Collections.Generic;

namespace SixScreenControllerApi.Models
{
    public class ScreenTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ScreenTemplateElement> ScreenTemplateElements { get; set; } = new List<ScreenTemplateElement>(6);
    }
}
