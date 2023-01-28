using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Six_Screens_Controller.Models;
using Six_Screens_Controller.Converters;


//TODO: исправить добавление одинаковых элементов
namespace Six_Screens_Controller
{
    /// <summary>
    /// Window for adding playlists
    /// </summary>
    public partial class AddPlaylistWindow : Window
    {
        /// <summary>
        /// <see cref="PlaylistElement"/>s for adding to <see cref="Playlist"/>
        /// </summary>
        public ObservableCollection<PlaylistElement> elements = new ObservableCollection<PlaylistElement>();
        /// <summary>
        /// Playlist title
        /// </summary>
        public string PlaylistTitle { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref = "AddPlaylistWindow"/> class
        /// </summary>
        public AddPlaylistWindow()
        {
            InitializeComponent();
            Loaded += AddPlaylist_Loaded;
            elemetsList.DataContext = elements;
        }

        private void AddPlaylist_Loaded(object sender, RoutedEventArgs e)
        {
            Binding OkIsEnabledBinding = new Binding
            {
                ElementName = "playlistTitle",
                Path = new PropertyPath("Text.Length"),
                Converter = new IntToBoolConverter()
            };

            Ok.SetBinding(IsEnabledProperty, OkIsEnabledBinding);
        }

        private void addElement_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog 
            { 
                Multiselect = true 
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string path = fileName;
                    string exp = path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                    if (Utils.ImageExp.Contains(exp))
                        elements.Add(new PlaylistElement { Path = fileName.Replace("\\", "/") });

                    if (Utils.VideoExp.Contains(exp))
                    {
                        MediaElement video = new MediaElement
                        {
                            Source = new Uri(path, UriKind.Absolute),
                            LoadedBehavior = MediaState.Manual,
                            UnloadedBehavior = MediaState.Manual,
                            Volume = 0
                        };
                        video.MediaOpened += new System.Windows.RoutedEventHandler(Media_MediaOpened);

                        video.Play();
                        while (!video.NaturalDuration.HasTimeSpan) { }  //NaturalDuration is equal to Automatic without this line
                        double dur = video.NaturalDuration.TimeSpan.TotalSeconds;
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

        private void Media_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
