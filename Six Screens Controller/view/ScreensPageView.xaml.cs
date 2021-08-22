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
    public partial class ScreensPageView : UserControl
    {
        public ScreenTemplate CurrentScreenTemplate { get; set; }

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
        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ScreenTemplate template = await Utils.GetRequestScreens();
            Utils.PostRequestScreens(template);
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
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!string.IsNullOrEmpty(CurrentScreenTemplate.ScreenTemplateElements[i].Path))
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetScreenTemplateElement(int screenNumber, ScreenTemplateElement element)
        {
            if (element != null && element.Path != null)
            {
                CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1] = element;
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
        }
    }
}