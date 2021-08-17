using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace Six_Screens_Controller.view
{
    /// <summary>
    /// Логика взаимодействия для ScreensPageView.xaml
    /// </summary>
    public partial class ScreensPageView : UserControl
    {
        public ScreenTemplate CurrentScreenTemplate { get; set; }
        public bool IsChangedTemplate { get; set; } = false;

        public ScreensPageView(ScreenTemplate screenTemplate)
        {
            InitializeComponent();
            SetScreenTemplate(screenTemplate);
        }

        private void ChooseElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                for (int i = 0; i < Elements.Children.Count; i++)
                {
                    (Elements.Children[i] as ListViewItem).IsSelected = false;
                }
            } (sender as ListViewItem).IsSelected = !(sender as ListViewItem).IsSelected;
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                    ScreenTemplateElement screen = new ScreenTemplateElement { Path = file, IsPlaylist = false, ScreenNumber = Convert.ToInt32(((ListViewItem)sender).Uid) };

                    Utils.PutRequestScreens(screen.ScreenNumber, screen);
                    Utils.RefreshRequest(Convert.ToInt32(((ListViewItem)sender).Uid));
                }
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

                    foreach (object i in Elements.Children)
                    {
                        if ((i as ListViewItem).IsSelected)
                        {
                            int screenNumber = Convert.ToInt32(((ListViewItem)i).Uid);
                            ScreenTemplateElement screen = new ScreenTemplateElement { ScreenNumber = screenNumber, IsPlaylist = false, Path = pickedFile };

                            Utils.PutRequestScreens(screen.ScreenNumber, screen);
                            Utils.RefreshRequest(screenNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PickAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (object i in Elements.Children)
            {
                (i as ListViewItem).IsSelected = true;
            }
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            IsChangedTemplate = true;
            Thread.Sleep(5);
            SetScreenTemplate(CurrentScreenTemplate);
            Utils.RefreshRequest();
        }
        private void Screen_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string pickedFile;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    int screenNumber = Convert.ToInt32(((ListViewItem)sender).Uid);
                    pickedFile = openFileDialog.FileName;
                    ScreenTemplateElement screen = new ScreenTemplateElement { ScreenNumber = screenNumber, Path = pickedFile, IsPlaylist = false };

                    Utils.PutRequestScreens(screenNumber, screen);
                    Utils.RefreshRequest(screenNumber);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetScreenTemplate(ScreenTemplate screenTemplate)
        {
            CurrentScreenTemplate = screenTemplate;
            IsChangedTemplate = false;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (CurrentScreenTemplate.ScreenTemplateElements[i].IsPlaylist == false)
                    {
                        string exp = CurrentScreenTemplate.ScreenTemplateElements[i].Path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                        if (Utils.imageExp.Contains(exp))
                        {
                            Image img = Utils.CreateImage(CurrentScreenTemplate.ScreenTemplateElements[i].Path);
                            (Elements.Children[i] as ListViewItem).Content = img;
                        }
                        else if (Utils.videoExp.Contains(exp))
                        {
                            MediaElement video = Utils.CreateVideo(CurrentScreenTemplate.ScreenTemplateElements[i].Path);
                            (Elements.Children[i] as ListViewItem).Content = video;
                        }
                        else if (exp == "gif")
                        {
                            MediaElement video = Utils.CreateVideo(CurrentScreenTemplate.ScreenTemplateElements[i].Path);
                            (Elements.Children[i] as ListViewItem).Content = video;
                        }
                    }
                    else
                    {
                        SetPlaylist(CurrentScreenTemplate.ScreenTemplateElements[i].Path, Convert.ToInt32((Elements.Children[i] as ListViewItem).Uid));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void SetPlaylist(string json, int screenNumber)
        {
            Playlist playlist;
            int id = ((dynamic)JsonConvert.DeserializeObject(json)).id;
            playlist = await Utils.GetRequestPlaylist(id);

            await Task.Run(() =>
            {
                for (int i = 0; i < playlist.PlaylistElements.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Image img = Utils.CreateImage(playlist.PlaylistElements[i].Path);
                        (Elements.Children[screenNumber - 1] as ListViewItem).Content = img;
                    });

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (stopwatch.Elapsed < TimeSpan.FromSeconds(playlist.PlaylistElements[i].Duration))
                    {
                        Thread.Sleep(1);
                        if (IsChangedTemplate)
                            goto Finish;
                    }
                }

            Finish:;
            });
        }

        public void SetScreenTemplateElement(int screenNumber, ScreenTemplateElement element)
        {
            CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1] = element;
            IsChangedTemplate = false;

            if (CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].IsPlaylist == false)
            {
                string exp = CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                if (Utils.imageExp.Contains(exp))
                {
                    Image img = Utils.CreateImage(CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path);
                    (Elements.Children[screenNumber - 1] as ListViewItem).Content = img;
                }
                else if (Utils.videoExp.Contains(exp))
                {
                    MediaElement video = Utils.CreateVideo(CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path);
                    (Elements.Children[screenNumber - 1] as ListViewItem).Content = video;
                }
                else if (exp == "gif")
                {
                    MediaElement video = Utils.CreateVideo(CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path);
                    (Elements.Children[screenNumber - 1] as ListViewItem).Content = video;
                }
            }
            else
            {
                SetPlaylist(CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path, screenNumber);
            }
        }
    }
}