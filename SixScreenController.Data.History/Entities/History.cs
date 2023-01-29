using System;
using System.ComponentModel.DataAnnotations.Schema;
using SixScreenController.Data.Templates.Entities;

namespace SixScreenController.Data.History.Entities
{
    public class History
    {
        public Guid Id { get; set; }

        public DateTime Changed { get; set; }

        [ForeignKey(nameof(ScreenTemplate))]
        public int SceenTemplateId { get; set; }

        public ScreenTemplate ScreenTemplate { get; set; }
    }
}
