using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Six_Screens_Controller
{
    public partial class ScreenTemplateElementControl : UserControl
    {
        public string ElementPath { get; set; }
        public int ElementDefaultId { get; set; } = -1;

        public ScreenTemplateElementControl()
        {
            InitializeComponent();
            Loaded += ScreenTemplateElementControl_Load;
        }

        private void ScreenTemplateElementControl_Load(object sender, RoutedEventArgs e)
        {
            using PlaylistContext db = new PlaylistContext();
            PlaylistScreen.ItemsSource = db.Playlists.Include(x => x.PlaylistElements).ToList();
            if (ElementDefaultId != -1)
                PlaylistScreen.SelectedValue = ElementDefaultId;
        }


        private void ElementBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                ElementPathBox.Text = openFileDialog.FileName;
        }
    }
}
