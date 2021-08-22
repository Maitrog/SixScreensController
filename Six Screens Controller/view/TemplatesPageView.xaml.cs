using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Six_Screens_Controller.view
{
    public partial class TemplatesPageView : UserControl
    {
        public ScreenTemplate ScreenTemplate { get; set; }

        public TemplatesPageView()
        {
            InitializeComponent();
            Loaded += TemplatesPage_Loaded;
        }

        private async void TemplatesPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                templateList.ItemsSource = await Utils.GetRequestScreenTemplates();
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
                templateList.ItemsSource = await Utils.GetRequestScreenTemplates();
            }
        }

        private async void removeTemplate_Click(object sender, RoutedEventArgs e)
        {
            object ScreenTemplate = templateList.SelectedItem;
            if (ScreenTemplate != null)
            {
                Utils.DeleteRequestScreenTemplates((ScreenTemplate as ScreenTemplate).Id);
            }
            templateList.ItemsSource = await Utils.GetRequestScreenTemplates();
        }

        private async void templateList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ScreenTemplate = await Utils.GetRequestScreenTemplates((templateList.SelectedItem as ScreenTemplate).Id);

            try
            {
                Utils.PostRequestScreens(ScreenTemplate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void changeTemplate_Click(object sender, RoutedEventArgs e)
        {
            ScreenTemplate = await Utils.GetRequestScreenTemplates((templateList.SelectedItem as ScreenTemplate).Id);
            ChangeTemplateWindow changeTemplateWindow = new ChangeTemplateWindow(ScreenTemplate);
            Utils.PutRequestScreenTemplates(ScreenTemplate.Id, changeTemplateWindow.ScreenTemplate);
            templateList.ItemsSource = await Utils.GetRequestScreenTemplates();
        }
    }
}
