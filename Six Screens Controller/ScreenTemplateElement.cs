using System;
using System.Collections.Generic;
using System.Text;

namespace Six_Screens_Controller
{
    public class ScreenTemplateElement
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsPlaylist { get; set; } = false;

        public ScreenTemplate ScreenTemplate { get; set; }
        public int ScreenTemplateId { get; set; }

        public override bool Equals(object obj)
        {
            if((obj as ScreenTemplateElement) != null)
                return (obj as ScreenTemplateElement).Id == Id && (obj as ScreenTemplateElement).Path == Path && (obj as ScreenTemplateElement).IsPlaylist == IsPlaylist;

            return false;
        }
    }
}
