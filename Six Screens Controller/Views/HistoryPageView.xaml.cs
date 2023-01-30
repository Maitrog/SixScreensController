using Microsoft.EntityFrameworkCore;
using SixScreenController.Data.History;
using SixScreenController.Data.History.Entities;
using SixScreenController.Data.Templates.Entities;
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

namespace Six_Screens_Controller.Views
{
    /// <summary>
    /// Interaction logic for HistoryControl.xaml
    /// </summary>
    public partial class HistoryPageView : UserControl
    {
        public ScreenTemplate ScreenTemplate { get; set; }

        public HistoryPageView()
        {
            InitializeComponent();
            Loaded += HistoryPage_Loaded;
        }

        private async void HistoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            using HistoryDbContext historyContext = new HistoryDbContext();
            historyList.ItemsSource = await historyContext.History.Include(x => x.ScreenTemplate)
                .OrderByDescending(x => x.Changed).ToListAsync();
        }

        private void HistoryList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            using HistoryDbContext historyContext = new HistoryDbContext();
            ScreenTemplate = historyContext.History.Include(x => x.ScreenTemplate)
                .Include(x => x.ScreenTemplate.ScreenTemplateElements)
                .FirstOrDefault(x => x.Id == (historyList.SelectedItem as History).Id).ScreenTemplate;

            try
            {
                Utils.PostRequestScreensAsync(ScreenTemplate);
                MessageBox.Show("Шаблон успешно установлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
