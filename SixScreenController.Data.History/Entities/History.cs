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

        [NotMapped]
        public string Title
        {
            get
            {
                if (ScreenTemplate.Title.Length > 83)
                {
                    return ScreenTemplate.Title[..80] + "...";
                }
                return ScreenTemplate.Title;
            }
        }

        [NotMapped]
        public string ChangedDisplay
        {
            get
            {
                return Changed.ToString("dd.MM.yyyy HH:mm");
            }
        }
    }
}
