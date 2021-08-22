using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public partial class AddTemplateWindow : Window
    {
        public ScreenTemplate ScreenTemplate { get; private set; }

        public AddTemplateWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ScreenTemplate = new ScreenTemplate() { Title = title.Text };

            for (int i = 4; i < StackPanel.Children.Count; i += 2)
            {
                if ((StackPanel.Children[i] as ScreenTemplateElementControl).IsPlaylistScreen.IsChecked == false)
                {
                    ScreenTemplate.ScreenTemplateElements.Add(new ScreenTemplateElement()
                    {
                        Path = (StackPanel.Children[i] as ScreenTemplateElementControl).ElementPath,
                        ScreenNumber = ScreenTemplate.ScreenTemplateElements.Count + 1
                    });
                }
                else if ((StackPanel.Children[i] as ScreenTemplateElementControl).IsPlaylistScreen.IsChecked == true)
                {
                    ScreenTemplate.ScreenTemplateElements.Add(new ScreenTemplateElement()
                    {
                        Path = JsonConvert.SerializeObject((StackPanel.Children[i] as ScreenTemplateElementControl).PlaylistScreen.SelectedItem as Playlist),
                        IsPlaylist = true,
                        ScreenNumber = ScreenTemplate.ScreenTemplateElements.Count + 1
                    });
                }
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
