using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Six_Screens_Controller
{
    public partial class AddPlaylistWindow : Window
    {
        public ObservableCollection<PlaylistElement> elements = new ObservableCollection<PlaylistElement>();
        public string PlaylistTitle { get; set; }

        public AddPlaylistWindow()
        {
            InitializeComponent();
            Loaded += AddPlaylist_Loaded;
            elemetsList.DataContext = elements;
        }

        public void AddPlaylist_Loaded(object sender, RoutedEventArgs e)
        {
            Binding OkIsEnabledBinding = new Binding();
            OkIsEnabledBinding.ElementName = "playlistTitle";
            OkIsEnabledBinding.Path = new PropertyPath("Text.Length");
            OkIsEnabledBinding.Converter = new IntToBoolConverter();

            Ok.SetBinding(IsEnabledProperty, OkIsEnabledBinding);
        }

        private void addElement_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string path = fileName;
                    string exp = path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                    if (MainWindow.imageExp.Contains(exp))
                        elements.Add(new PlaylistElement { Path = openFileDialog.FileName.Replace("\\", "/") });

                    if (MainWindow.videoExp.Contains(exp))
                    {
                        MediaElement video = new MediaElement();
                        video.Source = new Uri(path, UriKind.Absolute);
                        video.MediaOpened += new System.Windows.RoutedEventHandler(media_MediaOpened);
                        video.LoadedBehavior = MediaState.Manual;
                        video.UnloadedBehavior = MediaState.Manual;
                        video.Volume = 0;

                        video.Play();
                        while (!video.NaturalDuration.HasTimeSpan) { }  //без этой строчки NaturalDuration не успевает вычислиться, и равен Automatic
                        double dur = video.NaturalDuration.TimeSpan.TotalMilliseconds;
                        video.Close();

                        elements.Add(new PlaylistElement { Path = openFileDialog.FileName.Replace("\\", "/"), Duration = Convert.ToInt32(dur) });
                    }
                }

            }
        }

        private void removeElement_Click(object sender, RoutedEventArgs e)
        {
            int size = elemetsList.SelectedCells.Count;
            if (size > 0)
                for (int i = 0; i < size; size -= 2)
                    elements.Remove(elemetsList.SelectedCells[i].Item as PlaylistElement);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void media_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        { 
        }
    }
}
