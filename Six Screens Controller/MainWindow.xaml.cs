using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Six_Screens_Controller
{
    public partial class MainWindow : Window
    {
        Config Config { get; set; }
        public string DefaultImage { get; set; }
        public static readonly string[] imageExp = new string[] { "jpg", "jepg", "bmp", "png", "webp" };
        public static readonly string[] videoExp = new string[] { "mp4", "avi", "mpeg", "mkv", "3gp", "3g2" };

        public MainWindow()
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            if (Config.DefaultImage == "")
            {
                Config.DefaultImage = "assets/Emblem_of_the_Russian_Ground_Forces.jpg";
            }
            DefaultImage = Config.DefaultImage;

            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TemplateContext db = new TemplateContext())
                templateList.ItemsSource = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).ToList();

                using(PlaylistContext db = new PlaylistContext())
                playlistsList.ItemsSource = db.Playlists.Include(x=>x.PlaylistElements).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string exp = files[0].Split("\\").LastOrDefault().Split('.').LastOrDefault();

                    if (imageExp.Contains(exp))
                    {
                        Image img = CreateImage(files[0]);

                        (sender as CheckBox).Content = img;
                        PutRequest(Convert.ToInt32(((CheckBox)sender).Uid), files[0], "img");
                    }

                    else if (videoExp.Contains(exp))
                    {
                        MediaElement video = CreateVideo(files[0]);

                        (sender as CheckBox).Content = video;
                        PutRequest(Convert.ToInt32(((CheckBox)sender).Uid), files[0], "vid");
                    }

                    else if(exp == "gif")
                    {
                        MediaElement video = CreateVideo(files[0]);

                        (sender as CheckBox).Content = video;
                        PutRequest(Convert.ToInt32(((CheckBox)sender).Uid), files[0], "gif");
                    }
                }
                RefreshRequest();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                pickedFile.Text = openFileDialog.FileName;
        }


        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string exp = pickedFile.Text.Split("\\").LastOrDefault().Split('.').LastOrDefault();

                if (imageExp.Contains(exp))
                {

                    foreach (var i in wrapPanel.Children)
                    {
                        if ((i as CheckBox).IsChecked == true)
                        {
                            Image img = CreateImage(pickedFile.Text);
                            (i as CheckBox).Content = img;
                            (i as CheckBox).IsChecked = false;
                            PutRequest(Convert.ToInt32(((CheckBox)i).Uid), pickedFile.Text, "img");
                        }
                    }
                }

                else if (videoExp.Contains(exp))
                {
                    foreach (var i in wrapPanel.Children)
                    {

                        if ((i as CheckBox).IsChecked == true)
                        {
                            MediaElement video = CreateVideo(pickedFile.Text);
                            (i as CheckBox).Content = video;
                            (i as CheckBox).IsChecked = false;
                            PutRequest(Convert.ToInt32(((CheckBox)i).Uid), pickedFile.Text, "vid");
                        }
                    }
                }

                else if(exp == "gif")
                {
                    foreach (var i in wrapPanel.Children)
                    {

                        if ((i as CheckBox).IsChecked == true)
                        {
                            MediaElement video = CreateVideo(pickedFile.Text);
                            (i as CheckBox).Content = video;
                            (i as CheckBox).IsChecked = false;
                            PutRequest(Convert.ToInt32(((CheckBox)i).Uid), pickedFile.Text, "gif");
                        }
                    }
                }
                RefreshRequest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pickAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in wrapPanel.Children)
            {
                (i as CheckBox).IsChecked = true;
            }
        }

        private void addTemplate_Click(object sender, RoutedEventArgs e)
        {
            AddTemplateWindow addTemplateWindow = new AddTemplateWindow();
            using (TemplateContext db = new TemplateContext())
            {
                if (addTemplateWindow.ShowDialog() == true)
                {
                    db.ScreenTemplates.Add(addTemplateWindow.ScreenTemplate);
                    db.SaveChanges();

                    var templates = db.ScreenTemplates.ToList();
                    templateList.ItemsSource = templates;
                }
            }
        }

        private void removeTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (templateList.SelectedItems.Count > 0)
            {
                var selectedTemplate = templateList.SelectedItem;
                using (TemplateContext db = new TemplateContext())
                {
                    if (selectedTemplate != null)
                    {
                        ScreenTemplate screenTemplate = db.ScreenTemplates.Where(x => x.Id == (selectedTemplate as ScreenTemplate).Id).FirstOrDefault();
                        for (int i = 0; i < screenTemplate.ScreenTemplateElements.Count; i++)
                            db.ScreenTemplateElements.Remove(screenTemplate.ScreenTemplateElements[i]);
                        db.ScreenTemplates.Remove(screenTemplate);
                    }
                    db.SaveChanges();

                    var templates = db.ScreenTemplates.ToList();
                    templateList.ItemsSource = templates;
                }
            }
        }

        private void templateList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var selectedTemplate = templateList.SelectedItem as ScreenTemplate;
            

            var screen = wrapPanel.Children;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (selectedTemplate.ScreenTemplateElements[i].IsPlaylist == false)
                    {
                        string exp = selectedTemplate.ScreenTemplateElements[i].Path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                        if (imageExp.Contains(exp))
                        {
                            Image img = CreateImage(selectedTemplate.ScreenTemplateElements[i].Path);

                            (screen[i] as CheckBox).Content = img;
                            PutRequest(i + 1, selectedTemplate.ScreenTemplateElements[i].Path, "img");
                        }
                        else if (videoExp.Contains(exp))
                        {
                            MediaElement video = CreateVideo(selectedTemplate.ScreenTemplateElements[i].Path);

                            (screen[i] as CheckBox).Content = video;
                            PutRequest(i + 1, selectedTemplate.ScreenTemplateElements[i].Path, "vid");
                        }
                        else if(exp == "gif")
                        {
                            MediaElement video = CreateVideo(selectedTemplate.ScreenTemplateElements[i].Path);

                            (screen[i] as CheckBox).Content = video;
                            PutRequest(i + 1, selectedTemplate.ScreenTemplateElements[i].Path, "gif");
                        }
                    }
                    else
                    {
                        Image img = CreateImage(@"C:\Users\Mihay\Documents\pet-projects\Six Screens Controller\Six Screens Controller\assets\playlist.jpg");

                        (screen[i] as CheckBox).Content = img;
                        PutRequestPlaylist(i + 1, selectedTemplate.ScreenTemplateElements[i].Path);
                    }
                }
                RefreshRequest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void changeTemplate_Click(object sender, RoutedEventArgs e)
        {
            ChangeTemplateWindow changeTemplateWindow = new ChangeTemplateWindow(templateList.SelectedItem as ScreenTemplate);
            using (TemplateContext db = new TemplateContext())
            {
                if (changeTemplateWindow.ShowDialog() == true)
                {
                    var temp = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).Where(x => x.Id == changeTemplateWindow.ScreenTemplate.Id).FirstOrDefault();
                    temp.Title = changeTemplateWindow.ScreenTemplate.Title;
                    for (int i = 0; i < changeTemplateWindow.ScreenTemplate.ScreenTemplateElements.Count; i++)
                    {

                        if (!temp.ScreenTemplateElements[i].Equals(changeTemplateWindow.ScreenTemplate.ScreenTemplateElements[i]))
                        {
                            temp.ScreenTemplateElements.RemoveAt(i);
                            temp.ScreenTemplateElements.Insert(i, changeTemplateWindow.ScreenTemplate.ScreenTemplateElements[i]);
                        }

                    }
                    db.SaveChanges();

                    var templates = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).ToList();
                    templateList.ItemsSource = templates;
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog() == true)
            {
                if (!Config.Equals(settingsWindow.config))
                {

                    if (Config.Host != settingsWindow.config.Host)
                    {
                        string serverConfig = File.ReadAllText("config.txt");
                        string serverConfigHost = serverConfig.Split("\r\n").Where(x => x.Contains("HOST")).FirstOrDefault();
                        string newServerConfigHost = $"HOST = {settingsWindow.config.Host}";
                        serverConfig = serverConfig.Replace(serverConfigHost, newServerConfigHost);
                        File.WriteAllText("config.txt", serverConfig);
                    }
                    if(Config.Port != settingsWindow.config.Port)
                    {
                        string serverConfig = File.ReadAllText("config.txt");
                        string serverConfigPort = serverConfig.Split("\r\n").Where(x => x.Contains("PORT")).FirstOrDefault();
                        string newServerConfigPort = $"PORT = {settingsWindow.config.Port}";
                        serverConfig = serverConfig.Replace(serverConfigPort, newServerConfigPort);
                        File.WriteAllText("config.txt", serverConfig);
                    }

                    Config = settingsWindow.config;

                    File.WriteAllText("config.json", JsonConvert.SerializeObject(Config));
                    
                    MessageBox.Show("Для вступления изменений в силу перезапустите приложение");
                }
            }
        }

        private void removePlaylist_Click(object sender, RoutedEventArgs e)
        {
            using (PlaylistContext db = new PlaylistContext())
            {
                Playlist removedPlaylist = db.Playlists.First(x => x.Id == (playlistsList.SelectedItem as Playlist).Id);
                db.Playlists.Remove(removedPlaylist);
                db.SaveChanges();

                playlistsList.ItemsSource = db.Playlists.ToList();
            }
        }

        private void addPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylistWindow addPlaylistWindow = new AddPlaylistWindow();
            if(addPlaylistWindow.ShowDialog() == true)
            {
                using (PlaylistContext db = new PlaylistContext())
                {
                    Playlist pl = new Playlist();
                    pl.Title = addPlaylistWindow.PlaylistTitle;
                    for(int i = 0; i < addPlaylistWindow.elements.Count; i++)
                    {
                        var elem = db.PlaylistElements.Where(x => x.Path == (addPlaylistWindow.elements[i].Path) && x.Duration == (addPlaylistWindow.elements[i].Duration)).ToList();
                        if (elem.Count == 0)
                        {
                            db.PlaylistElements.Add(addPlaylistWindow.elements[i]);
                            db.SaveChanges();
                            pl.PlaylistElements.Add(addPlaylistWindow.elements[i]);
                        }
                        else
                        {
                            pl.PlaylistElements.Add(elem[0]);
                        }
                    }
                    db.Playlists.Add(pl);
                    db.SaveChanges();

                    playlistsList.ItemsSource = db.Playlists.Include(x => x.PlaylistElements).ToList();
                }
            }
        }

        private Image CreateImage(string path)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path, UriKind.Absolute);
            bmp.EndInit();

            Image img = new Image();
            img.Source = bmp;
            img.Width = 590;
            img.Margin = new Thickness(0, 5, 0, 5);
            img.Stretch = Stretch.Uniform;

            return img;
        }

        private MediaElement CreateVideo(string path)
        {
            MediaElement video = new MediaElement();
            video.Source = new Uri(path, UriKind.Absolute);
            video.Width = 590;
            video.Stretch = Stretch.Uniform;
            video.Margin = new Thickness(0, 5, 0, 5);
            video.Volume = 0;
            video.LoadedBehavior = MediaState.Play;

            return video;
        }

        private bool PutRequest(int number, string file, string mediaType, int duration = 1)
        {

            string route = $"screen/{number}";
            string url = $"{Config.Protocol}://{Config.Host}:{Config.Port}/{route}";
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

        private bool PutRequestPlaylist(int number, string jsonStr)
        {
            string route = $"playlist/{number}";
            string url = $"{Config.Protocol}://{Config.Host}:{Config.Port}/{route}";

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

        private void RefreshRequest()
        {
            string route = $"refresh";
            string url = $"{Config.Protocol}://{Config.Host}:{Config.Port}/{route}";

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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
