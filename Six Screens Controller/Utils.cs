using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Six_Screens_Controller
{
    /// <summary>
    /// Exposes static methods for create image and video; put, post, get, delete request on api; refresh request
    /// </summary>
    internal static class Utils
    {
        public static readonly string[] imageExp = new string[] { "jpg", "jpeg", "bmp", "png", "webp" };
        public static readonly string[] videoExp = new string[] { "mp4", "avi", "mpeg", "mkv", "3gp", "3g2" };
        public static Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Create a new instance of the <see cref="Image"/>
        /// </summary>
        /// <param name="path">Path to image</param>
        /// <returns><see cref="Image"/></returns>
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
        /// <summary>
        /// Create a instance of the <see cref="MediaElement"/>
        /// </summary>
        /// <param name="path">Path to video or gif</param>
        /// <returns><see cref="MediaElement"/></returns>
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

        // Screens controller methods

        /// <summary>
        /// Put request to /api/screens/{screenNumber} – change current content on screen with number {screenNumber}
        /// </summary>
        /// <param name="screenNumber">Screen number</param>
        /// <param name="element">Element of the screen template which should be installeted</param>
        public static async void PutRequestScreensAsync(int screenNumber, ScreenTemplateElement element)
        {
            string url = GetUrl($"screens/{screenNumber}");
            HttpResponseMessage response = await client.PutAsJsonAsync(url, element);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Post request to /api/screens – change current template
        /// </summary>
        /// <param name="screenTemplate">Screen template which should be installeted</param>
        public static async void PostRequestScreensAsync(ScreenTemplate screenTemplate)
        {
            try
            {
                string url = GetUrl("screens");
                HttpResponseMessage response = await client.PostAsJsonAsync(url, screenTemplate);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
            }
        }

        /// <summary>
        /// Get request to /api/screens – get current content
        /// </summary>
        /// <returns>Current <see cref="ScreenTemplate"/></returns>
        public static async Task<ScreenTemplate> GetRequestScreensAsync()
        {
            string url = GetUrl("screens");
            string response = await client.GetStringAsync(url);
            ScreenTemplate template = JsonConvert.DeserializeObject<ScreenTemplate>(response);
            return template;
        }

        /// <summary>
        /// Get request to /api/screens/{screenNumber} – get current content on screen with number {screenNumber}
        /// </summary>
        /// <param name="screenNumber">Screen number</param>
        /// <returns>Current <see cref="ScreenTemplateElement"/> on screen</returns>
        public static async Task<ScreenTemplateElement> GetRequestScreensAsync(int screenNumber)
        {
            string url = GetUrl($"screens/{screenNumber}");
            string response = await client.GetStringAsync(url);
            ScreenTemplateElement element = JsonConvert.DeserializeObject<ScreenTemplateElement>(response);
            return element;
        }

        // Playlist controller methods

        /// <summary>
        /// Get async request to /api/playlist – get all playlists from database
        /// </summary>
        /// <returns><see cref="List{T}"/></returns>
        public static async Task<List<Playlist>> GetRequestPlaylistAsync()
        {
            string url = GetUrl("playlist");
            string response = await client.GetStringAsync(url);
            List<Playlist> playlists = JsonConvert.DeserializeObject<List<Playlist>>(response);
            return playlists;
        }

        /// <summary>
        /// Get async request to /api/playlist/{id} – get playlist with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Playlist"/></returns>
        public static async Task<Playlist> GetRequestPlaylistAsync(int id)
        {
            string url = GetUrl($"playlist/{id}");
            string response = await client.GetStringAsync(url);
            Playlist playlist = JsonConvert.DeserializeObject<Playlist>(response);
            return playlist;
        }

        /// <summary>
        /// Get request to /api/playlist – get all playlists from database
        /// </summary>
        /// <returns><see cref="List{T}"/></returns>
        public static List<Playlist> GetRequestPlaylist()
        {
            string url = GetUrl($"playlist");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var response = streamReader.ReadToEnd();
                List<Playlist> playlists = JsonConvert.DeserializeObject<List<Playlist>>(response);
                return playlists;
            }
        }

        /// <summary>
        /// Post async request to /api/playlist – add playlist in database
        /// </summary>
        /// <param name="playlist"></param>
        public static async void PostRequestPlaylistAsync(Playlist playlist)
        {
            string url = GetUrl("playlist");
            HttpResponseMessage response = await client.PostAsJsonAsync(url, playlist);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Post request to /api/playlist – add playlist in database
        /// </summary>
        /// <param name="playlist"></param>
        public static void PostRequestPlaylist(Playlist playlist)
        {
            string url = GetUrl("playlist");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(playlist));
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        /// <summary>
        /// Delete async request to /api/playlist/{id} – delete playlist with {id}
        /// </summary>
        /// <param name="id"></param>
        public static async void DeleteRequestPlaylistAsync(int id)
        {
            string url = GetUrl($"playlist/{id}");
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Delete request to /api/playlist/{id} – delete playlist with {id}
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteRequestPlaylist(int id)
        {
            string url = GetUrl($"playlist/{id}");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "DELETE";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(id);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        // ScreenTemplates controller methods

        /// <summary>
        /// Get request to /api/screentemplates – get all <see cref="ScreenTemplate"/> from database
        /// </summary>
        /// <returns><see cref="List{T}"/></returns>
        public static async Task<List<ScreenTemplate>> GetRequestScreenTemplatesAsync()
        {
            string url = GetUrl("screentemplates");
            string response = await client.GetStringAsync(url);

            List<ScreenTemplate> screenTemplates = JsonConvert.DeserializeObject<List<ScreenTemplate>>(response);
            return screenTemplates;
        }

        /// <summary>
        /// Get request to /api/screentemplates/{id} – get <see cref="ScreenTemplate"/> with {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ScreenTemplate"/></returns>
        public static async Task<ScreenTemplate> GetRequestScreenTemplatesAsync(int id)
        {
            string url = GetUrl($"screentemplates/{id}");
            string response = await client.GetStringAsync(url);

            ScreenTemplate screenTemplates = JsonConvert.DeserializeObject<ScreenTemplate>(response);
            return screenTemplates;
        }

        /// <summary>
        /// Post async request to /api/screentemplates – add screen template in database
        /// </summary>
        /// <param name="screenTemplate"></param>
        public static async void PostRequestScreenTemplatesAsync(ScreenTemplate screenTemplate)
        {
            string url = GetUrl("screentemplates");
            HttpResponseMessage response = await client.PostAsJsonAsync(url, screenTemplate);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Post request to /api/screentemplates – add screen template in database
        /// </summary>
        /// <param name="screenTemplate"></param>
        public static void PostRequestScreenTemplates(ScreenTemplate screenTemplate)
        {
            string url = GetUrl("screentemplates");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(screenTemplate));
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        /// <summary>
        /// Delete async request to /api/screentemplates/{id} – delete screen template with {id} from database
        /// </summary>
        /// <param name="id"></param>
        public static async void DeleteRequestScreenTemplatesAsync(int id)
        {
            string url = GetUrl($"screentemplates/{id}");
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Delete request to /api/screentemplates/{id} – delete screen template with {id} from database
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteRequestScreenTemplates(int id)
        {
            string url = GetUrl($"screentemplates/{id}");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "DELETE";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(id);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        /// <summary>
        /// Put request to /api/screentemplates/{id} – add screen template in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="screenTemplate"></param>
        public static async void PutRequestScreenTemplatesAsync(int id, ScreenTemplate screenTemplate)
        {
            string url = GetUrl($"screentemplates/{id}");
            HttpResponseMessage response = await client.PutAsJsonAsync(url, screenTemplate);
            response.EnsureSuccessStatusCode();
        }

        // Other request

        /// <summary>
        /// Request to /refresh – refresh screen
        /// </summary>
        /// <param name="screenNumber">Screen number which should be update</param>
        public static async void RefreshRequestAsync(int screenNumber = 0)
        {
            HubConnection HubConnection = new HubConnectionBuilder()
            .WithUrl($"{config.Protocol}://{config.Host}:{config.Port}/refresh")
            .Build();
            HubConnection.On<int>("Refresh", screenNumber => Console.WriteLine(screenNumber));
            await HubConnection.StartAsync();

            await HubConnection.SendAsync("SendRefresh", screenNumber);
        }

        private static string GetUrl(string pathRoute)
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
            string route = $"api/{pathRoute}";
            string url = $"{config.Protocol}://{config.Host}:{config.Port}/{route}";
            return url;
        }
    }
}
