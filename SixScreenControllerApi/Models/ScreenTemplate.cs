using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Models
{
    public class ScreenTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ScreenTemplateElement> ScreenTemplateElements { get; set; } = new List<ScreenTemplateElement>(6);
    }
}
