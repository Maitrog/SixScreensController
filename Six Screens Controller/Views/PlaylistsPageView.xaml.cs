using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using SixScreenController.Data.Templates.Entities;

namespace Six_Screens_Controller.Views
{
    /// <summary>
    /// Page with playlists
    /// </summary>
    public partial class PlaylistsPageView : UserControl
    {
        /// <summary>
        /// Gets or sets selected playlist
        /// </summary>
        public ScreenTemplateElement Playlist { get; set; } = new ScreenTemplateElement();

        /// <summary>
        /// Initializes a new instance of the <see cref = "PlaylistsPageView"/> class.
        /// </summary>
        public PlaylistsPageView()
        {
            InitializeComponent();
            Loaded += PlaylistsPage_Loaded;
        }

        private async void PlaylistsPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                playlistsList.ItemsSource = await Utils.GetRequestPlaylistAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void addPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylistWindow addPlaylistWindow = new AddPlaylistWindow();
            if (addPlaylistWindow.ShowDialog() == true)
            {
                Playlist pl = new Playlist
                {
                    Title = addPlaylistWindow.PlaylistTitle
                };
                for (int i = 0; i < addPlaylistWindow.elements.Count; i++)
                {
                    pl.PlaylistElements.Add(addPlaylistWindow.elements[i]);
                }
                Utils.PostRequestPlaylist(pl);

                playlistsList.ItemsSource = await Utils.GetRequestPlaylistAsync();
            }
        }


        private async void removePlaylist_Click(object sender, RoutedEventArgs e)
        {
            Utils.DeleteRequestPlaylist((playlistsList.SelectedItem as Playlist).Id);
            playlistsList.ItemsSource = await Utils.GetRequestPlaylistAsync();
        }

        private async void SetPlaylist_Click(object sender, RoutedEventArgs e)
        {
            Playlist playlist = await Utils.GetRequestPlaylistAsync((playlistsList.SelectedItem as Playlist).Id);

            Playlist.IsPlaylist = true;
            Playlist.Id = playlist.Id;
            Playlist.Path = JsonConvert.SerializeObject(playlist);
            Playlist.ScreenNumber = Convert.ToInt32((sender as MenuItem).Uid);

            Utils.PutRequestScreensAsync(Playlist.ScreenNumber, Playlist);
        }
    }
}
