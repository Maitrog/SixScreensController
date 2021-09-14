using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Six_Screens_Controller
{
    internal static class Utils
    {
        public static readonly string[] imageExp = new string[] { "jpg", "jpeg", "bmp", "png", "webp" };
        public static readonly string[] videoExp = new string[] { "mp4", "avi", "mpeg", "mkv", "3gp", "3g2" };
        public static Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        private static readonly HttpClient client = new HttpClient();

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

        /// <summary>
        /// Screens controller methods
        /// </summary>

        public static async void PutRequestScreensAsync(int screenNumber, ScreenTemplateElement element)
        {
            string url = GetUrl($"screens/{screenNumber}");
            HttpResponseMessage response = await client.PutAsJsonAsync(url, element);
            response.EnsureSuccessStatusCode();
        }

        public static async void PostRequestScreensAsync(ScreenTemplate screenTemplate)
        {
            string url = GetUrl("screens");
            HttpResponseMessage response = await client.PostAsJsonAsync(url, screenTemplate);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<ScreenTemplate> GetRequestScreensAsync()
        {
            string url = GetUrl("screens");
            string response = await client.GetStringAsync(url);
            ScreenTemplate template = JsonConvert.DeserializeObject<ScreenTemplate>(response);
            return template;
        }

        public static async Task<ScreenTemplateElement> GetRequestScreensAsync(int screenNumber)
        {
            string url = GetUrl($"screens/{screenNumber}");
            string response = await client.GetStringAsync(url);
            ScreenTemplateElement element = JsonConvert.DeserializeObject<ScreenTemplateElement>(response);
            return element;
        }

        /// <summary>
        /// Playlist controller methods
        /// </summary>

        public static async Task<List<Playlist>> GetRequestPlaylistAsync()
        {
            string url = GetUrl("playlist");
            string response = await client.GetStringAsync(url);
            List<Playlist> playlists = JsonConvert.DeserializeObject<List<Playlist>>(response);
            return playlists;
        }

        public static async Task<Playlist> GetRequestPlaylistAsync(int id)
        {
            string url = GetUrl($"playlist/{id}");
            string response = await client.GetStringAsync(url);
            Playlist playlist = JsonConvert.DeserializeObject<Playlist>(response);
            return playlist;
        }

        public static async void PostRequestPlaylistAsync(Playlist playlist)
        {
            string url = GetUrl("playlist");
            HttpResponseMessage response = await client.PostAsJsonAsync(url, playlist);
            response.EnsureSuccessStatusCode();
        }

        public static async void DeleteRequestPlaylistAsync(int id)
        {
            string url = GetUrl($"playlist/{id}");
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// ScreenTemplates controller methods
        /// </summary>

        public static async Task<List<ScreenTemplate>> GetRequestScreenTemplatesAsync()
        {
            string url = GetUrl("screentemplates");
            string response = await client.GetStringAsync(url);

            List<ScreenTemplate> screenTemplates = JsonConvert.DeserializeObject<List<ScreenTemplate>>(response);
            return screenTemplates;
        }

        public static async Task<ScreenTemplate> GetRequestScreenTemplatesAsync(int id)
        {
            string url = GetUrl($"screentemplates/{id}");
            string response = await client.GetStringAsync(url);

            ScreenTemplate screenTemplates = JsonConvert.DeserializeObject<ScreenTemplate>(response);
            return screenTemplates;
        }

        public static async void PostRequestScreenTemplatesAsync(ScreenTemplate screenTemplate)
        {
            string url = GetUrl("screentemplates");
            HttpResponseMessage response = await client.PostAsJsonAsync(url, screenTemplate);
            response.EnsureSuccessStatusCode();
        }

        public static async void DeleteRequestScreenTemplatesAsync(int id)
        {
            string url = GetUrl($"screentemplates/{id}");
            HttpResponseMessage response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        public static async void PutRequestScreenTemplatesAsync(int id, ScreenTemplate screenTemplate)
        {
            string url = GetUrl($"screentemplates/{id}");
            HttpResponseMessage response = await client.PutAsJsonAsync(url, screenTemplate);
            response.EnsureSuccessStatusCode();
        }

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
