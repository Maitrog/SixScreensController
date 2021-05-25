using Microsoft.EntityFrameworkCore;
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
    /// <summary>
    /// Логика взаимодействия для TemplatesPageView.xaml
    /// </summary>
    public partial class TemplatesPageView : UserControl
    {
        public ScreenTemplate ScreenTemplate { get; set; }
        public bool IsDestroy { get; set; } = false;
        public bool IsChangeTemplate { get; set; } = false;

        public TemplatesPageView()
        {
            InitializeComponent();
            Loaded += TemplatesPage_Loaded;
        }

        private void TemplatesPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TemplateContext db = new TemplateContext())
                    templateList.ItemsSource = db.ScreenTemplates.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addTemplate_Click(object sender, RoutedEventArgs e)
        {
            AddTemplateWindow addTemplateWindow = new AddTemplateWindow();
            using (TemplateContext db = new TemplateContext())
            {
                if (addTemplateWindow.ShowDialog() == true)
                {
                    db.ScreenTemplates.Add(addTemplateWindow.ScreenTemplate);
                    db.SaveChanges();

                    templateList.ItemsSource = db.ScreenTemplates.ToList();
                }
            }
        }

        private void removeTemplate_Click(object sender, RoutedEventArgs e)
        {

            var ScreenTemplate = templateList.SelectedItem;
            using (TemplateContext db = new TemplateContext())
            {
                if (ScreenTemplate != null)
                {
                    ScreenTemplate screenTemplate = db.ScreenTemplates.Where(x => x.Id == (ScreenTemplate as ScreenTemplate).Id).FirstOrDefault();
                    for (int i = 0; i < screenTemplate.ScreenTemplateElements.Count; i++)
                        db.ScreenTemplateElements.Remove(screenTemplate.ScreenTemplateElements[i]);
                    db.ScreenTemplates.Remove(screenTemplate);
                }
                db.SaveChanges();

                templateList.ItemsSource = db.ScreenTemplates.ToList();

            }
        }

        private void templateList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            using (TemplateContext db = new TemplateContext())
                ScreenTemplate = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).Where(x=> x.Id == (templateList.SelectedItem as ScreenTemplate).Id).FirstOrDefault();
            
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!ScreenTemplate.ScreenTemplateElements[i].IsPlaylist)
                    {
                        string exp = ScreenTemplate.ScreenTemplateElements[i].Path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                        if (Utils.imageExp.Contains(exp))
                        {
                            Utils.PutRequest(i + 1, ScreenTemplate.ScreenTemplateElements[i].Path, "img");
                        }
                        else if (Utils.videoExp.Contains(exp))
                        {
                            Utils.PutRequest(i + 1, ScreenTemplate.ScreenTemplateElements[i].Path, "vid");
                        }
                        else if (exp == "gif")
                        {
                            Utils.PutRequest(i + 1, ScreenTemplate.ScreenTemplateElements[i].Path, "gif");
                        }
                    }
                    else
                    {
                        Utils.PutRequestPlaylist(i + 1, ScreenTemplate.ScreenTemplateElements[i].Path);
                    }
                }
                Utils.RefreshRequest();
                IsChangeTemplate = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void changeTemplate_Click(object sender, RoutedEventArgs e)
        {
            using (TemplateContext db = new TemplateContext())
                ScreenTemplate = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).Where(x => x.Id == (templateList.SelectedItem as ScreenTemplate).Id).FirstOrDefault();

            ChangeTemplateWindow changeTemplateWindow = new ChangeTemplateWindow(ScreenTemplate);
            using (TemplateContext db = new TemplateContext())
            {
                if (changeTemplateWindow.ShowDialog() == true)
                {
                    var temp = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).Where(x => x.Id == changeTemplateWindow.ScreenTemplate.Id).FirstOrDefault();
                    temp.Title = changeTemplateWindow.ScreenTemplate.Title;
                    for (int i = 0; i < changeTemplateWindow.ScreenTemplate.ScreenTemplateElements.Count; i++)
                    {

                        if (!temp.ScreenTemplateElements[i].Equals(changeTemplateWindow.ScreenTemplate.ScreenTemplateElements[i]))
                        {
                            temp.ScreenTemplateElements.RemoveAt(i);
                            temp.ScreenTemplateElements.Insert(i, changeTemplateWindow.ScreenTemplate.ScreenTemplateElements[i]);
                        }

                    }
                    db.SaveChanges();

                    templateList.ItemsSource = db.ScreenTemplates.Include(x => x.ScreenTemplateElements).ToList();
                }
            }
        }
    }
}
