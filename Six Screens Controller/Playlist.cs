using Newtonsoft.Json;
using Six_Screens_Controller.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Six_Screens_Controller
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PlaylistElement> PlaylistElements { get; set; } = new List<PlaylistElement>();
    }
}
