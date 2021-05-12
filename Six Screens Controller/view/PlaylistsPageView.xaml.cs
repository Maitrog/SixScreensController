using Microsoft.EntityFrameworkCore;
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
    /// <summary>
    /// Логика взаимодействия для PlaylistsPageView.xaml
    /// </summary>
    public partial class PlaylistsPageView : UserControl
    {
        public PlaylistsPageView()
        {
            InitializeComponent();
            Loaded += PlaylistsPage_Loaded;
        }

        private void PlaylistsPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (PlaylistContext db = new PlaylistContext())
                    playlistsList.ItemsSource = db.Playlists.Include(x => x.PlaylistElements).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylistWindow addPlaylistWindow = new AddPlaylistWindow();
            if (addPlaylistWindow.ShowDialog() == true)
            {
                using (PlaylistContext db = new PlaylistContext())
                {
                    Playlist pl = new Playlist();
                    pl.Title = addPlaylistWindow.PlaylistTitle;
                    for (int i = 0; i < addPlaylistWindow.elements.Count; i++)
                    {
                        var elem = db.PlaylistElements.Where(x => x.Path == (addPlaylistWindow.elements[i].Path) && x.Duration == (addPlaylistWindow.elements[i].Duration)).ToList();
                        if (elem.Count == 0)
                        {
                            db.PlaylistElements.Add(addPlaylistWindow.elements[i]);
                            db.SaveChanges();
                            pl.PlaylistElements.Add(addPlaylistWindow.elements[i]);
                        }
                        else
                        {
                            pl.PlaylistElements.Add(elem[0]);
                        }
                    }
                    db.Playlists.Add(pl);
                    db.SaveChanges();

                    playlistsList.ItemsSource = db.Playlists.Include(x => x.PlaylistElements).ToList();
                }
            }
        }

        private void removePlaylist_Click(object sender, RoutedEventArgs e)
        {
            using (PlaylistContext db = new PlaylistContext())
            {
                Playlist removedPlaylist = db.Playlists.First(x => x.Id == (playlistsList.SelectedItem as Playlist).Id);
                db.Playlists.Remove(removedPlaylist);
                db.SaveChanges();

                playlistsList.ItemsSource = db.Playlists.ToList();
            }
        }
    }
}
