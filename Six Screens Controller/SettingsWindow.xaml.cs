﻿using Microsoft.Win32;
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

        private void serverBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                server.Text = openFileDialog.FileName;

        }

        private void interpreterBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Выбор расположения Python";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = Directory.GetCurrentDirectory();

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Directory.GetCurrentDirectory();
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                interpreterPython.Text = dlg.FileName;
            }
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
