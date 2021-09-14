﻿using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


//TODO: исправить добавление одинаковых элементов
namespace Six_Screens_Controller
{
    public partial class AddPlaylistWindow : Window
    {
        public ObservableCollection<PlaylistElement> elements = new ObservableCollection<PlaylistElement>();
        public string PlaylistTitle { get; set; }

        public AddPlaylistWindow()
        {
            InitializeComponent();
            Loaded += AddPlaylist_Loaded;
            elemetsList.DataContext = elements;
        }

        public void AddPlaylist_Loaded(object sender, RoutedEventArgs e)
        {
            Binding OkIsEnabledBinding = new Binding
            {
                ElementName = "playlistTitle",
                Path = new PropertyPath("Text.Length"),
                Converter = new IntToBoolConverter()
            };

            Ok.SetBinding(IsEnabledProperty, OkIsEnabledBinding);
        }

        private void addElement_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog 
            { 
                Multiselect = true 
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string path = fileName;
                    string exp = path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                    if (Utils.imageExp.Contains(exp))
                        elements.Add(new PlaylistElement { Path = fileName.Replace("\\", "/") });

                    if (Utils.videoExp.Contains(exp))
                    {
                        MediaElement video = new MediaElement
                        {
                            Source = new Uri(path, UriKind.Absolute),
                            LoadedBehavior = MediaState.Manual,
                            UnloadedBehavior = MediaState.Manual,
                            Volume = 0
                        };
                        video.MediaOpened += new System.Windows.RoutedEventHandler(Media_MediaOpened);

                        video.Play();
                        while (!video.NaturalDuration.HasTimeSpan) { }  //без этой строчки NaturalDuration не успевает вычислиться, и равен Automatic
                        double dur = video.NaturalDuration.TimeSpan.TotalSeconds;
                        video.Close();

                        elements.Add(new PlaylistElement { Path = openFileDialog.FileName.Replace("\\", "/"), Duration = Convert.ToInt32(dur) });
                    }
                }

            }
        }

        private void removeElement_Click(object sender, RoutedEventArgs e)
        {
            int size = elemetsList.SelectedCells.Count;
            if (size > 0)
                for (int i = 0; i < size; size -= 2)
                    elements.Remove(elemetsList.SelectedCells[i].Item as PlaylistElement);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void Media_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
