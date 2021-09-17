using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Six_Screens_Controller
{


    /// <summary>
    /// Create settings window
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public Config config;

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
    }
}
