using System;
using System.Windows;
using System.Windows.Controls;
using SixScreenController.Data.Templates.Entities;

namespace Six_Screens_Controller.Views
{
    /// <summary>
    /// Page with templates
    /// </summary>
    public partial class TemplatesPageView : UserControl
    {
        /// <summary>
        /// Gets or sets selected template
        /// </summary>
        public ScreenTemplate ScreenTemplate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplatesPageView"/> class.
        /// </summary>
        public TemplatesPageView()
        {
            InitializeComponent();
            Loaded += TemplatesPage_Loaded;
        }

        private async void TemplatesPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                templateList.ItemsSource = await Utils.GetRequestScreenTemplatesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void addTemplate_Click(object sender, RoutedEventArgs e)
        {
            AddTemplateWindow addTemplateWindow = new AddTemplateWindow();

            if (addTemplateWindow.ShowDialog() == true)
            {
                Utils.PostRequestScreenTemplates(addTemplateWindow.ScreenTemplate);
                templateList.ItemsSource = await Utils.GetRequestScreenTemplatesAsync();
            }
        }

        private async void removeTemplate_Click(object sender, RoutedEventArgs e)
        {
            object ScreenTemplate = templateList.SelectedItem;
            if (ScreenTemplate != null)
            {
                Utils.DeleteRequestScreenTemplates((ScreenTemplate as ScreenTemplate).Id);
            }
            templateList.ItemsSource = await Utils.GetRequestScreenTemplatesAsync();
        }

        private async void templateList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ScreenTemplate = await Utils.GetRequestScreenTemplatesAsync((templateList.SelectedItem as ScreenTemplate).Id);

            try
            {
                Utils.PostRequestScreensAsync(ScreenTemplate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void changeTemplate_Click(object sender, RoutedEventArgs e)
        {
            ScreenTemplate = await Utils.GetRequestScreenTemplatesAsync((templateList.SelectedItem as ScreenTemplate).Id);
            ChangeTemplateWindow changeTemplateWindow = new ChangeTemplateWindow(ScreenTemplate);
            if (changeTemplateWindow.ShowDialog() == true)
            {
                Utils.PutRequestScreenTemplatesAsync(ScreenTemplate.Id, changeTemplateWindow.ScreenTemplate);
            }
            templateList.ItemsSource = await Utils.GetRequestScreenTemplatesAsync();

        }
    }
}
