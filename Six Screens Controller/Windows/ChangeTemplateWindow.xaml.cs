using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Six_Screens_Controller.Comparators;
using SixScreenController.Data.Templates.Entities;

namespace Six_Screens_Controller
{
    public partial class ChangeTemplateWindow : Window
    {
        public ScreenTemplate ScreenTemplate { get; private set; }

        public ChangeTemplateWindow(ScreenTemplate screenTemplate)
        {
            InitializeComponent();
            ScreenTemplate = screenTemplate;
            ScreenTemplate.ScreenTemplateElements.Sort(new ScreenTemplateElementCopm());

            title.Text = screenTemplate.Title;
            for (int i = 4; i < StackPanel.Children.Count; i += 2)
            {
                if (screenTemplate.ScreenTemplateElements[(i - 4) / 2].IsPlaylist)
                {
                    (StackPanel.Children[i] as ScreenTemplateElementControl).IsPlaylistScreen.IsChecked = true;
                    dynamic tmp = JsonConvert.DeserializeObject(screenTemplate.ScreenTemplateElements[(i - 4) / 2].Path);
                    (StackPanel.Children[i] as ScreenTemplateElementControl).ElementDefaultId = tmp.Id;
                }
                else
                {
                    (StackPanel.Children[i] as ScreenTemplateElementControl).ElementPathBox.Text = screenTemplate.ScreenTemplateElements[(i - 4) / 2].Path;
                }
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 4; i < StackPanel.Children.Count; i += 2)
            {
                if ((StackPanel.Children[i] as ScreenTemplateElementControl).IsPlaylistScreen.IsChecked == false)
                {
                    string path = (StackPanel.Children[i] as ScreenTemplateElementControl).ElementPath;
                    if (path != null && path != ScreenTemplate.ScreenTemplateElements[(i - 4) / 2].Path)
                    {
                        ScreenTemplate.ScreenTemplateElements[(i - 4) / 2] = new ScreenTemplateElement()
                        {
                            Path = (StackPanel.Children[i] as ScreenTemplateElementControl).ElementPath,
                            ScreenNumber = (i - 4) / 2
                        };
                    }
                }
                else if ((StackPanel.Children[i] as ScreenTemplateElementControl).IsPlaylistScreen.IsChecked == true)
                {
                    string path = JsonConvert.SerializeObject((StackPanel.Children[i] as ScreenTemplateElementControl).PlaylistScreen.SelectedItem as Playlist);
                    if (path != null && path != ScreenTemplate.ScreenTemplateElements[(i - 4) / 2].Path)
                    {
                        ScreenTemplate.ScreenTemplateElements[(i - 4) / 2] = new ScreenTemplateElement()
                        {
                            Path = path,
                            IsPlaylist = true,
                            ScreenNumber = (i - 4) / 2
                        };
                    }
                }
            }
            ScreenTemplate.Title = title.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
