using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Six_Screens_Controller.view
{
    /// <summary>
    /// Логика взаимодействия для ScreensPageView.xaml
    /// </summary>
    public partial class ScreensPageView : UserControl
    {
        public static readonly string[] imageExp = new string[] { "jpg", "jepg", "bmp", "png", "webp" };
        public static readonly string[] videoExp = new string[] { "mp4", "avi", "mpeg", "mkv", "3gp", "3g2" };
        public ScreenTemplate ScreenTemplateNow { get; set; }

        public ScreensPageView(ScreenTemplate screenTemplate)
        {
            ScreenTemplateNow = screenTemplate;
            InitializeComponent();
            Loaded += ScreensPageView_Loaded;
        }

        private void ScreensPageView_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetScreenTemplate(ScreenTemplateNow);
        }

        private void ChooseElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
                for (int i = 0; i < Elements.Children.Count; i++)
                    (Elements.Children[i] as ListViewItem).IsSelected = false;

            (sender as ListViewItem).IsSelected = !(sender as ListViewItem).IsSelected;
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string exp = files[0].Split("\\").LastOrDefault().Split('.').LastOrDefault().ToLower();

                    if (imageExp.Contains(exp))
                    {
                        Image img = CreateImage(files[0]);

                        (sender as ListViewItem).Content = img;
                        PutRequest(Convert.ToInt32(((ListViewItem)sender).Uid), files[0], "img");
                    }

                    else if (videoExp.Contains(exp))
                    {
                        MediaElement video = CreateVideo(files[0]);

                        (sender as ListViewItem).Content = video;
                        PutRequest(Convert.ToInt32(((ListViewItem)sender).Uid), files[0], "vid");
                    }

                    else if (exp == "gif")
                    {
                        MediaElement video = CreateVideo(files[0]);

                        (sender as ListViewItem).Content = video;
                        PutRequest(Convert.ToInt32(((ListViewItem)sender).Uid), files[0], "gif");
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
            string pickedFile;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    pickedFile = openFileDialog.FileName;
                    string exp = pickedFile.Split("\\").LastOrDefault().Split('.').LastOrDefault().ToLower();

                    if (imageExp.Contains(exp))
                    {

                        foreach (var i in Elements.Children)
                        {
                            if ((i as ListViewItem).IsSelected == true)
                            {
                                Image img = CreateImage(pickedFile);
                                (i as ListViewItem).Content = img;
                                (i as ListViewItem).IsSelected = false;
                                PutRequest(Convert.ToInt32(((ListViewItem)i).Uid), pickedFile, "img");
                            }
                        }
                    }

                    else if (videoExp.Contains(exp))
                    {
                        foreach (var i in Elements.Children)
                        {

                            if ((i as ListViewItem).IsSelected == true)
                            {
                                MediaElement video = CreateVideo(pickedFile);
                                (i as ListViewItem).Content = video;
                                (i as ListViewItem).IsSelected = false;
                                PutRequest(Convert.ToInt32(((ListViewItem)i).Uid), pickedFile, "vid");
                            }
                        }
                    }

                    else if (exp == "gif")
                    {
                        foreach (var i in Elements.Children)
                        {

                            if ((i as ListViewItem).IsSelected == true)
                            {
                                MediaElement video = CreateVideo(pickedFile);
                                (i as ListViewItem).Content = video;
                                (i as ListViewItem).IsSelected = false;
                                PutRequest(Convert.ToInt32(((ListViewItem)i).Uid), pickedFile, "gif");
                            }
                        }
                    }
                    RefreshRequest();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PickAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Elements.Children)
            {
                (i as ListViewItem).IsSelected = true;
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
            img.Margin = new Thickness(0, 5, 0, 5);
            img.Stretch = Stretch.Uniform;

            return img;
        }
        private MediaElement CreateVideo(string path)
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
            Config  config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
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

        public static void RefreshRequest()
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
            string route = $"refresh";
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

        public void SetScreenTemplate(ScreenTemplate screenTemplate)
        {
            ScreenTemplateNow = screenTemplate;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (ScreenTemplateNow.ScreenTemplateElements[i].IsPlaylist == false)
                    {
                        string exp = ScreenTemplateNow.ScreenTemplateElements[i].Path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                        if (imageExp.Contains(exp))
                        {
                            Image img = CreateImage(ScreenTemplateNow.ScreenTemplateElements[i].Path);

                            (Elements.Children[i] as ListViewItem).Content = img;
                            PutRequest(i + 1, ScreenTemplateNow.ScreenTemplateElements[i].Path, "img");
                        }
                        else if (videoExp.Contains(exp))
                        {
                            MediaElement video = CreateVideo(ScreenTemplateNow.ScreenTemplateElements[i].Path);

                            (Elements.Children[i] as ListViewItem).Content = video;
                            PutRequest(i + 1, ScreenTemplateNow.ScreenTemplateElements[i].Path, "vid");
                        }
                        else if (exp == "gif")
                        {
                            MediaElement video = CreateVideo(ScreenTemplateNow.ScreenTemplateElements[i].Path);

                            (Elements.Children[i] as ListViewItem).Content = video;
                            PutRequest(i + 1, ScreenTemplateNow.ScreenTemplateElements[i].Path, "gif");
                        }
                    }
                    else
                    {
                        Image img = CreateImage(@"C:\Users\Mihay\Documents\pet-projects\Six Screens Controller\Six Screens Controller\assets\playlist.jpg");

                        (Elements.Children[i] as ListViewItem).Content = img;
                        PutRequestPlaylist(i + 1, ScreenTemplateNow.ScreenTemplateElements[i].Path);
                    }
                }
                RefreshRequest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshRequest();
        }
    }
}