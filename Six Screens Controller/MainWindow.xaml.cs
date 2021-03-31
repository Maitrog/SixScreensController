using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Six_Screens_Controller;
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
    public class ScreenTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Screen1 { get; set; }
        public string Screen2 { get; set; }
        public string Screen3 { get; set; }
        public string Screen4 { get; set; }
        public string Screen5 { get; set; }
        public string Screen6 { get; set; }

    }

    class TemplateContext : DbContext
    {
        public DbSet<ScreenTemplate> ScreenTemplates { get; set; }

        public TemplateContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=VkScreenTemplate.db");
        }
    }


    public partial class MainWindow : Window
    {
        Config config { get; set; }
        public string DefaultImage { get; set; }
        readonly string[] imageExp = new string[] { "jpg", "jepg", "bmp", "png", "gif", "webp" };
        readonly string[] videoExp = new string[] { "mkv", "mp4", "avi", "3gp", "webm", "mpeg", "3g2" };

        public MainWindow()
        {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            if (config.DefaultImage == null)
            {
                config.DefaultImage = "assets/Emblem_of_the_Russian_Ground_Forces.jpg";
            }
            DefaultImage = config.DefaultImage;
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TemplateContext db = new TemplateContext())
                {
                    List<ScreenTemplate> templates = db.ScreenTemplates.ToList();
                    templateList.ItemsSource = templates;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string[] path = files[0].Split("\\");
                    string[] file = path[path.Length - 1].Split(".");
                    string exp = file[file.Length - 1];

                    if (imageExp.Contains(exp))
                    {
                        Image img = CreateImage(files[0]);

                        (sender as CheckBox).Content = img;
                    }

                    else if (videoExp.Contains(exp))
                    {
                        MediaElement video = CreateVideo(files[0]);

                        (sender as CheckBox).Content = video;
                    }

                    put_request(Convert.ToInt32(((CheckBox)sender).Uid), files[0]);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pickFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                pickedFile.Text = openFileDialog.FileName;
        }


        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] path = pickedFile.Text.Split("\\");
                string[] file = path[path.Length - 1].Split(".");
                string exp = file[file.Length - 1];

                if (imageExp.Contains(exp))
                {
                    Image img = CreateImage(pickedFile.Text);

                    foreach (var i in wrapPanel.Children)
                    {
                        if ((i as CheckBox).IsChecked == true)
                        {
                            (i as CheckBox).Content = img;
                            (i as CheckBox).IsChecked = false;
                            put_request(Convert.ToInt32(((CheckBox)i).Uid), pickedFile.Text);
                        }
                    }
                }

                else if (videoExp.Contains(exp))
                {
                    foreach (var i in wrapPanel.Children)
                    {
                        MediaElement video = CreateVideo(pickedFile.Text);

                        if ((i as CheckBox).IsChecked == true)
                        {
                            (i as CheckBox).Content = video;
                            (i as CheckBox).IsChecked = false;
                            put_request(Convert.ToInt32(((CheckBox)i).Uid), pickedFile.Text);
                        }
                    }
                }
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
            string[] data = new string[] { selectedTemplate.Screen1,
                selectedTemplate.Screen2,
                selectedTemplate.Screen3,
                selectedTemplate.Screen4,
                selectedTemplate.Screen5,
                selectedTemplate.Screen6 };

            var screen = wrapPanel.Children;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    string[] path = data[i].Split("\\");
                    string[] file = path[path.Length - 1].Split(".");
                    string exp = file[file.Length - 1];
                    if (imageExp.Contains(exp))
                    {
                        Image img = CreateImage(data[i]);

                        (screen[i] as CheckBox).Content = img;
                        put_request(i + 1, data[i]);
                    }
                    else if (videoExp.Contains(exp))
                    {
                        MediaElement video = CreateVideo(data[i]);

                        (screen[i] as CheckBox).Content = video;
                        put_request(i + 1, data[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            img.Width = 500;
            img.Margin = new Thickness(0, 5, 0, 5);
            img.Stretch = Stretch.Uniform;

            return img;
        }

        private MediaElement CreateVideo(string path)
        {
            MediaElement video = new MediaElement();
            video.Source = new Uri(path, UriKind.Absolute);
            video.Width = 500;
            video.Stretch = Stretch.Uniform;
            video.Margin = new Thickness(0, 5, 0, 5);
            video.Volume = 0;
            video.LoadedBehavior = MediaState.Play;

            return video;
        }

        private void put_request(int number, string file)
        {
            string route = $"screen/{number}";
            string url = $"{config.Protocol}://{config.Host}:{config.Port}/{route}";
            StringBuilder filePath = new StringBuilder(file);
            for (int i = 0; i < filePath.Length; i++)
                if (filePath[i] == '\\')
                {
                    filePath[i] = '/';
                }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            string json_string = $"{{\n \"url\" : \"{filePath}\" \n}}";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json_string);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        private void changeTemplate_Click(object sender, RoutedEventArgs e)
        {
            ChangeTemplateWindow changeTemplateWindow = new ChangeTemplateWindow(templateList.SelectedItem as ScreenTemplate);
            using (TemplateContext db = new TemplateContext())
            {
                if (changeTemplateWindow.ShowDialog() == true)
                {
                    var temp = db.ScreenTemplates.Where(x => x.Id == changeTemplateWindow.ScreenTemplate.Id).FirstOrDefault();
                    temp.Title = changeTemplateWindow.ScreenTemplate.Title;
                    temp.Screen1 = changeTemplateWindow.ScreenTemplate.Screen1;
                    temp.Screen2 = changeTemplateWindow.ScreenTemplate.Screen2;
                    temp.Screen3 = changeTemplateWindow.ScreenTemplate.Screen3;
                    temp.Screen4 = changeTemplateWindow.ScreenTemplate.Screen4;
                    temp.Screen5 = changeTemplateWindow.ScreenTemplate.Screen5;
                    temp.Screen6 = changeTemplateWindow.ScreenTemplate.Screen6;
                    db.SaveChanges();

                    var templates = db.ScreenTemplates.ToList();
                    templateList.ItemsSource = templates;
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            if(settingsWindow.ShowDialog() == true)
            {
                if (!config.Equals(settingsWindow.config))
                {
                    MessageBox.Show("Для вступления изменений в силу перезапустите приложение");
                    config = settingsWindow.config;
                }
                File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}
