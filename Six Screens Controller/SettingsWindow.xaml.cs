using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Six_Screens_Controller
{
    /// <summary>
    /// Window with settings
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Installed configuration
        /// </summary>
        public Config config;

        /// <summary>
        /// Initialize a new instance of the <see cref = "SettingsWindow"/> class
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();

            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            this.DataContext = config;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void browseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                defaultImage.Text = openFileDialog.FileName;
        }
    }
}
