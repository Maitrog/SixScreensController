using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string args = "";
            if (value != null)
            {
                args += ((string[])value)[0];
                for(int i = 1; i < ((string[])value).Length; i++)
                {
                    args += " " + ((string[])value)[i];
                }
            }
            return args;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] args;
            args = ((string)value).Split(" ");
            return args;
        }
    }

    public partial class SettingsWindow : Window
    {
        public Config config;

        public SettingsWindow()
        {
            InitializeComponent();

            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            this.DataContext = config;
        }

        private void serverBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                server.Text = openFileDialog.FileName;
        }

        private void interpreterBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                interpreterPython.Text = openFileDialog.FileName;
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
