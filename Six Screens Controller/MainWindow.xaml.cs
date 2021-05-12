using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Six_Screens_Controller.view;
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
        private static readonly Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
        private static ScreenTemplate ScreenTemplateNow { get; set; } = new ScreenTemplate()
        {
            ScreenTemplateElements = new List<ScreenTemplateElement> {
            new ScreenTemplateElement() { ScreenNumber = 1, Path = config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 2, Path = config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 3, Path = config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 4, Path = config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 5, Path = config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 6, Path = config.DefaultImage }
            }
        };
        private ScreensPageView screensPage = new ScreensPageView(ScreenTemplateNow);

        public MainWindow()
        { 
            Grid.SetColumn(screensPage, 2);
            InitializeComponent();
            MainGrid.Children.Insert(2, screensPage);
        }

        private void TemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainGrid.Children[2].GetType() != Type.GetType("Six_Screens_Controller.view.TemplatesPageView"))
            {
                TemplatesPageView templatesPageControl = new TemplatesPageView();
                Grid.SetColumn(templatesPageControl, 2);
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, templatesPageControl);

                ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
            }
        }

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainGrid.Children[2].GetType() == Type.GetType("Six_Screens_Controller.view.TemplatesPageView"))
            {
                if ((MainGrid.Children[2] as TemplatesPageView).ScreenTemplate != null)
                {
                    ScreenTemplateNow = (MainGrid.Children[2] as TemplatesPageView).ScreenTemplate;
                    screensPage.SetScreenTemplate(ScreenTemplateNow);
                }
            }
            if (MainGrid.Children[2].GetType() != Type.GetType("Six_Screens_Controller.view.PlaylistsPageView"))
            {
                PlaylistsPageView playlistsPageControl = new PlaylistsPageView();
                Grid.SetColumn(playlistsPageControl, 2);
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, playlistsPageControl);

                ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
            }
        }

        private void ScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainGrid.Children[2].GetType() == Type.GetType("Six_Screens_Controller.view.TemplatesPageView"))
            {
                if ((MainGrid.Children[2] as TemplatesPageView).ScreenTemplate != null)
                {
                    ScreenTemplateNow = (MainGrid.Children[2] as TemplatesPageView).ScreenTemplate;
                    screensPage.SetScreenTemplate(ScreenTemplateNow);
                }
            }
            if (MainGrid.Children[2].GetType() != Type.GetType("Six_Screens_Controller.view.ScreensPageView"))
            {
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, screensPage);

                ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
            }
        }

 
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Config Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
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
                    if (Config.Port != settingsWindow.config.Port)
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
