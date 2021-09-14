using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;

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
