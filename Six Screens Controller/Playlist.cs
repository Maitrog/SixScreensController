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


        public string JsonString()
        {
            string mediaType = "";
            string json = "{\r\n";
            json += $"\"id\": {Id},\r\n" +
                $"\"items\": [\r\n";
            for (int i = 0; i < PlaylistElements.Count; i++)
            {
                if (PlaylistElements[i].Path.Split("/").LastOrDefault().Split(".").LastOrDefault() == "gif")
                    mediaType = "gif";
                else if (Utils.imageExp.Contains(PlaylistElements[i].Path.Split("/").LastOrDefault().Split(".").LastOrDefault()))
                    mediaType = "img";
                else if (Utils.videoExp.Contains(PlaylistElements[i].Path.Split("/").LastOrDefault().Split(".").LastOrDefault()))
                    mediaType = "vid";
                if (i != PlaylistElements.Count - 1)
                    json += $"{{\r\n\"location\": \"{PlaylistElements[i].Path}\",\r\n" +
                        $"\"duration\": {PlaylistElements[i].Duration},\r\n" +
                        $"\"media_type\": \"{mediaType}\"\r\n}},\r\n";
                else
                    json += $"{{\r\n\"location\": \"{PlaylistElements[i].Path}\",\r\n" +
                        $"\"duration\": {PlaylistElements[i].Duration},\r\n" +
                        $"\"media_type\": \"{mediaType}\"\r\n}}\r\n";

            }

            json += "]\r\n}";

            return json;

        }
    }
}
