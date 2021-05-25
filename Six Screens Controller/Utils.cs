using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Six_Screens_Controller
{
    static class Utils
    {
        public static readonly string[] imageExp = new string[] { "jpg", "jepg", "bmp", "png", "webp" };
        public static readonly string[] videoExp = new string[] { "mp4", "avi", "mpeg", "mkv", "3gp", "3g2" };

        public static Image CreateImage(string path)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path, UriKind.Absolute);
            bmp.EndInit();

            Image img = new Image();
            img.Source = bmp;
            img.Margin = new Thickness(0, 5, 0, 5);
            img.Stretch = Stretch.Uniform;

            return img;
        }
        public static MediaElement CreateVideo(string path)
        {
            MediaElement video = new MediaElement();
            video.Source = new Uri(path, UriKind.Absolute);
            video.Stretch = Stretch.Uniform;
            video.Margin = new Thickness(0, 5, 0, 5);
            video.Volume = 0;
            video.LoadedBehavior = MediaState.Play;

            return video;
        }

        public static bool PutRequest(int number, string file, string mediaType, int duration = 1)
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
            string route = $"screen/{number}";
            string url = $"{config.Protocol}://{config.Host}:{config.Port}/{route}";
            file = file.Replace("\\", "/");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            string jsonString = $"{{\n \"location\": \"{file}\",\n" +
            $"\"duration\": {duration},\n" +
            $"\"media_type\": \"{mediaType}\"}}";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonString);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }

            return true;
        }

        public static bool PutRequestPlaylist(int number, string jsonStr)
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
            string route = $"playlist/{number}";
            string url = $"{config.Protocol}://{config.Host}:{config.Port}/{route}";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonStr);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }

            return true;
        }

        public static void RefreshRequest(int screen = -1)
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
            string route = "";
            if (screen == -1)
                route = $"refresh";
            else
                route = $"refresh/{screen}";
            string url = $"{config.Protocol}://{config.Host}:{config.Port}/{route}";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
    }
}
