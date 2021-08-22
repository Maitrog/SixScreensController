using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class PlaylistsPageView : UserControl
    {
        public ScreenTemplateElement Playlist { get; set; } = new ScreenTemplateElement();

        public PlaylistsPageView()
        {
            InitializeComponent();
            Loaded += PlaylistsPage_Loaded;
        }

        private async void PlaylistsPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                playlistsList.ItemsSource = await Utils.GetRequestPlaylist();
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

                playlistsList.ItemsSource = await Utils.GetRequestPlaylist();
            }
        }


        private async void removePlaylist_Click(object sender, RoutedEventArgs e)
        {
            Utils.DeleteRequestPlaylist((playlistsList.SelectedItem as Playlist).Id);
            playlistsList.ItemsSource = await Utils.GetRequestPlaylist();
        }

        //TODO: Сделать установку плейлиста на один экран
        private async void SetPlaylist_Click(object sender, RoutedEventArgs e)
        {
            Playlist playlist = await Utils.GetRequestPlaylist((playlistsList.SelectedItem as Playlist).Id);

            Playlist.IsPlaylist = true;
            Playlist.Id = playlist.Id;
            Playlist.Path = JsonConvert.SerializeObject(playlist);
            Playlist.ScreenNumber = Convert.ToInt32((sender as MenuItem).Uid);

            Utils.PutRequestScreens(Playlist.ScreenNumber, Playlist);
        }
    }
}
