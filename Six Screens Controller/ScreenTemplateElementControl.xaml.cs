using Microsoft.Win32;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Six_Screens_Controller
{
    public partial class ScreenTemplateElementControl : UserControl, INotifyPropertyChanged
    {
        private string elementPath;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ElementPath
        {
            get => elementPath;
            set
            {
                if (value != null)
                {
                    elementPath = value;
                    RaisePropertyChanged("ElementPath");
                }
            }
        }
        public int ElementDefaultId { get; set; } = -1;

        public ScreenTemplateElementControl()
        {
            InitializeComponent();
            Loaded += ScreenTemplateElementControl_Load;
        }

        private void ScreenTemplateElementControl_Load(object sender, RoutedEventArgs e)
        {
            PlaylistScreen.ItemsSource = Utils.GetRequestPlaylist();
            Playlist playlist = Utils.GetRequestPlaylist().FirstOrDefault(x => x.Id == ElementDefaultId);
            if (ElementDefaultId != -1)
            {
                PlaylistScreen.SelectedValue = ElementDefaultId;
            }
        }

        private void ElementBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                ElementPathBox.Text = openFileDialog.FileName;
        }
    }
}
