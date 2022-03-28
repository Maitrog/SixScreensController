using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Six_Screens_Controller.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Six_Screens_Controller.Models;


namespace Six_Screens_Controller
{
    public partial class MainWindow : Window
    {
        private static readonly Config _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
        private static HubConnection _hubConnection;
        private static readonly ScreenTemplate _screenTemplateNow = new ScreenTemplate()
        {
            ScreenTemplateElements = new List<ScreenTemplateElement> {
            new ScreenTemplateElement() { ScreenNumber = 1, Path = _config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 2, Path = _config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 3, Path = _config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 4, Path = _config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 5, Path = _config.DefaultImage },
            new ScreenTemplateElement() { ScreenNumber = 6, Path = _config.DefaultImage }
            }
        };
        private readonly ScreensPageView _screensPage = new ScreensPageView(_screenTemplateNow);
        private readonly PresentationPageView _presentationPageControl = new PresentationPageView();

        public MainWindow()
        {
            try
            {
                Grid.SetColumn(_screensPage, 2);
                InitializeComponent();
                Loaded += MainWindow_Loaded;
                MainGrid.Children.Insert(2, _screensPage);
            }
            catch (Exception)
            {

            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl($"{_config.Protocol}://{_config.Host}:{_config.Port}/refresh").WithAutomaticReconnect().Build();
            _hubConnection.On<int>("Refresh", screenNumber => Refresh(screenNumber));
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться к серверу. Проверьте параметры подключения.");
            }
        }

        private async void Refresh(int screenNumber)
        {
            if (screenNumber == 0)
            {
                ScreenTemplate screenTemplate = await Utils.GetRequestScreensAsync();
                _screensPage.SetScreenTemplate(screenTemplate);
            }
            else
            {
                ScreenTemplateElement screenTemplateElement = await Utils.GetRequestScreensAsync(screenNumber);
                _screensPage.SetScreenTemplateElement(screenNumber, screenTemplateElement);
            }
        }

        private void TemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainGrid.Children[2] is TemplatesPageView))
            {

                TemplatesPageView templatesPageControl = new TemplatesPageView();
                Grid.SetColumn(templatesPageControl, 2);
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, templatesPageControl);

                ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[4] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
            }
        }

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainGrid.Children[2] is PlaylistsPageView))
            {
                PlaylistsPageView playlistsPageControl = new PlaylistsPageView();
                Grid.SetColumn(playlistsPageControl, 2);
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, playlistsPageControl);

                ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[4] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
            }
        }

        private void ScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainGrid.Children[2] is ScreensPageView))
            {
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, _screensPage);

                ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[4] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
            }
        }

        private void PresentationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(MainGrid.Children[2] is PresentationPageView))
                {
                    Grid.SetColumn(_presentationPageControl, 2);
                    MainGrid.Children.RemoveAt(2);
                    MainGrid.Children.Insert(2, _presentationPageControl);

                    ((MainGrid.Children[0] as Grid).Children[1] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    ((MainGrid.Children[0] as Grid).Children[3] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    ((MainGrid.Children[0] as Grid).Children[4] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    ((MainGrid.Children[0] as Grid).Children[2] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Config Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            SettingsWindow settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog() == true)
            {
                if (Config.Background_1 != settingsWindow.config.Background_1)
                {
                    Utils.PostRequestBackgroundAsync(1, settingsWindow.config.Background_1);
                }
                if (Config.Background_2 != settingsWindow.config.Background_2)
                {
                    Utils.PostRequestBackgroundAsync(2, settingsWindow.config.Background_2);
                }
                if (Config.Background_3 != settingsWindow.config.Background_3)
                {
                    Utils.PostRequestBackgroundAsync(3, settingsWindow.config.Background_3);
                }
                if (Config.Background_4 != settingsWindow.config.Background_4)
                {
                    Utils.PostRequestBackgroundAsync(4, settingsWindow.config.Background_4);
                }
                if (Config.Background_5 != settingsWindow.config.Background_5)
                {
                    Utils.PostRequestBackgroundAsync(5, settingsWindow.config.Background_5);
                }
                if (Config.Background_6 != settingsWindow.config.Background_6)
                {
                    Utils.PostRequestBackgroundAsync(6, settingsWindow.config.Background_6);
                }
                File.WriteAllText("config.json", JsonConvert.SerializeObject(settingsWindow.config));
                if (!Config.Equals(settingsWindow.config))
                {
                    MessageBox.Show("Для вступления изменений в силу перезапустите приложение");
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            AboutUsWindow aboutUs = new AboutUsWindow();
            aboutUs.ShowDialog();
        }
    }
}
