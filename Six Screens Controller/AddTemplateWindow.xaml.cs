using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            ScreenTemplate = new ScreenTemplate()
            {
                Screen1 = screen1.Text,
                Screen2 = screen2.Text,
                Screen3 = screen3.Text,
                Screen4 = screen4.Text,
                Screen5 = screen5.Text,
                Screen6 = screen6.Text,
                Title = title.Text
            };

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
