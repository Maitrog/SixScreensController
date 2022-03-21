using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using Six_Screens_Controller.Models;

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

        private void browseBackground_1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                background_1.Text = openFileDialog.FileName;
        }

        private void browseBackground_2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                background_2.Text = openFileDialog.FileName;
        }

        private void browseBackground_3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                background_3.Text = openFileDialog.FileName;
        }

        private void browseBackground_4_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                background_4.Text = openFileDialog.FileName;
        }

        private void browseBackground_5_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                background_5.Text = openFileDialog.FileName;
        }

        private void browseBackground_6_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp;*.jpg;*.jpeg;*.png;*.webp)|*.bmp;*.jpg;*.jpeg;*.png;*.webp";
            if (openFileDialog.ShowDialog() == true)
                background_6.Text = openFileDialog.FileName;
        }
    }
}
