using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Six_Screens_Controller.Models;
using Six_Screens_Controller.Views;
using SixScreenController.Data.Templates.Entities;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Six_Screens_Controller
{
    public partial class MainWindow : Window
    {
        private static readonly Config _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
        private static HubConnection _hubConnection;
        private static readonly ScreenTemplate _screenTemplateNow = new ScreenTemplate(_config.DefaultImage);
        private readonly ScreensPageView _screensPage = new ScreensPageView(_screenTemplateNow);
        private readonly PresentationPageView _presentationPageControl = new PresentationPageView();
        private readonly bool[] _screenOnlineStatuses = { false, false, false, false, false, false };

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
            OnlineScreenAsync(CancellationToken.None);
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
                _screensPage.SetScreenTemplate(screenTemplate, true);
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
                ChangeButtonColor(3);
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
                ChangeButtonColor(4);
            }
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainGrid.Children[2] is HistoryPageView))
            {
                HistoryPageView historyPageView = new HistoryPageView();
                Grid.SetColumn(historyPageView, 2);
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, historyPageView);
                ChangeButtonColor(5);
            }
        }

        private void ScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(MainGrid.Children[2] is ScreensPageView))
            {
                MainGrid.Children.RemoveAt(2);
                MainGrid.Children.Insert(2, _screensPage);
                ChangeButtonColor(1);
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

                    ChangeButtonColor(2);
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

        private void ChangeButtonColor(int currentButton)
        {
            ((MainGrid.Children[0] as Grid).Children[currentButton] as Button).Background = new SolidColorBrush(Color.FromRgb(197, 197, 197));

            for (int i = 1; i < 6; i++)
            {
                if (i != currentButton)
                {
                    ((MainGrid.Children[0] as Grid).Children[i] as Button).Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                }
            }
        }

        private void OnlineScreenAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                HubConnection hubConnection = new HubConnectionBuilder().WithUrl($"{_config.Protocol}://{_config.Host}:{_config.Port}/refresh").WithAutomaticReconnect().Build();
                hubConnection.On<string>("Ping", screenNumber => _screenOnlineStatuses[int.Parse(screenNumber[..1]) - 1] = true);
                try
                {
                    await hubConnection.StartAsync();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось подключиться к серверу. Проверьте параметры подключения.");
                }

                while (true)
                {
                    for (int i = 1; i < 7; i++)
                    {
                        await hubConnection.SendAsync("Ping", $"{i}");
                    }
                    await Task.Delay(3000);
                    _screensPage.SetOnline(_screenOnlineStatuses);
                    for (int i = 0; i < 6; i++)
                    {
                        _screenOnlineStatuses[i] = false;
                    }
                }
            });
        }

        private void ImportDB_Click(object sender, RoutedEventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Zip (*.zip)|*.zip"
            };

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fileName = sfd.FileName;
                if (fileName.Split('.')[^1].ToLower() != "zip")
                {
                    fileName += ".zip";
                }

                using var fs = File.Create(fileName);
                using ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Update);
                AddEntryToArchive(archive, "History.db");
                AddEntryToArchive(archive, "Presentations.db");
                AddEntryToArchive(archive, "VkScreenController.db");
                AddDirectoryToArchive(archive, "Presentations");
            }
        }

        private static void AddDirectoryToArchive(ZipArchive archive, string dirName)
        {
            if (Directory.Exists(dirName))
            {
                var files = Directory.GetFiles(dirName);
                foreach (var file in files)
                {
                    AddEntryToArchive(archive, file);
                }

                var dirs = Directory.GetDirectories(dirName);
                foreach (var dir in dirs)
                {
                    AddDirectoryToArchive(archive, dir);
                }
            }
        }

        private static void AddEntryToArchive(ZipArchive archive, string entryName)
        {
            ZipArchiveEntry readmeEntry = archive.CreateEntry(entryName);
            using BinaryWriter writer = new BinaryWriter(readmeEntry.Open());
            writer.Write(File.ReadAllBytes(entryName));
        }

        private void ExportDB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Zip (*.zip)|*.zip"
            };

            string fileName;
            if (openFileDialog.ShowDialog() == true)
                fileName = openFileDialog.FileName;
        }
    }
}
