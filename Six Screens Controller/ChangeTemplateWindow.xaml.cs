using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для ChangeTemplateWindow.xaml
    /// </summary>
    public partial class ChangeTemplateWindow : Window
    {
        public ScreenTemplate ScreenTemplate { get; private set; }

        public ChangeTemplateWindow(ScreenTemplate screenTemplate)
        {
            InitializeComponent();
            ScreenTemplate = screenTemplate;
            title.Text = screenTemplate.Title;
            screen1.Text = screenTemplate.Screen1;
            screen2.Text = screenTemplate.Screen2;
            screen3.Text = screenTemplate.Screen3;
            screen4.Text = screenTemplate.Screen4;
            screen5.Text = screenTemplate.Screen5;
            screen6.Text = screenTemplate.Screen6;
        }
        private void Browse1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                screen1.Text = openFileDialog.FileName;
        }

        private void Browse2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                screen2.Text = openFileDialog.FileName;
        }

        private void Browse3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                screen3.Text = openFileDialog.FileName;
        }

        private void Browse4_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                screen4.Text = openFileDialog.FileName;
        }

        private void Browse5_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                screen5.Text = openFileDialog.FileName;
        }

        private void Browse6_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                screen6.Text = openFileDialog.FileName;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {

            ScreenTemplate.Screen1 = screen1.Text;
            ScreenTemplate.Screen2 = screen2.Text;
            ScreenTemplate.Screen3 = screen3.Text;
            ScreenTemplate.Screen4 = screen4.Text;
            ScreenTemplate.Screen5 = screen5.Text;
            ScreenTemplate.Screen6 = screen6.Text;
            ScreenTemplate.Title = title.Text;

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
