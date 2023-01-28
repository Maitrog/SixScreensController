using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using SixScreenController.Data.Templates.Entities;

namespace Six_Screens_Controller
{
    /// <summary>
    /// Window for adding templates
    /// </summary>
    public partial class AddTemplateWindow : Window
    {
        /// <summary>
        /// Created template
        /// </summary>
        public ScreenTemplate ScreenTemplate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AddTemplateWindow"/> class
        /// </summary>
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
