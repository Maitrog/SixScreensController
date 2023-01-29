using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Win32;
using Six_Screens_Controller.Converters;
using SixScreenController.Data.History;
using SixScreenController.Data.History.Entities;
using SixScreenController.Data.Templates.Entities;

namespace Six_Screens_Controller.Views
{
    /// <summary>
    /// Page with current screens
    /// </summary>
    public partial class ScreensPageView : UserControl
    {
        public ScreenTemplate CurrentScreenTemplate { get; set; } = new ScreenTemplate();
        private int clickedScreenNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref = "ScreensPageView"/> class with the specified <see cref="ScreenTemplate"/>
        /// </summary>
        /// <param name="screenTemplate"></param>
        public ScreensPageView(ScreenTemplate screenTemplate)
        {
            if (string.IsNullOrEmpty(screenTemplate.ScreenTemplateElements[0].Path) || !File.Exists(screenTemplate.ScreenTemplateElements[0].Path))
            {
                string defDir = Directory.GetCurrentDirectory();
                string defFile = $"{defDir}/assets/Default.jpg";
                for (int i = 0; i < 6; i++)
                {
                    screenTemplate.ScreenTemplateElements[i].Path = defFile;
                }
            }

            InitializeComponent();
            Loaded += ScreensPageView_Loaded;
            try
            {
                Utils.PostRequestScreensAsync(screenTemplate);
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message);
            }

            SetScreenTemplate(screenTemplate);
        }

        private void ScreensPageView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ListViewItem element in Elements.Children)
            {
                element.ContextMenu.Uid = element.Uid;
            }
        }

        private void ChooseElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                for (int i = 0; i < Elements.Children.Count; i++)
                {
                    (Elements.Children[i] as ListViewItem).IsSelected = false;
                }
            }

            (sender as ListViewItem).IsSelected = !(sender as ListViewItem).IsSelected;
        }

        private async void File_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                    ScreenTemplateElement screen = new ScreenTemplateElement { Path = file, IsPlaylist = false, ScreenNumber = Convert.ToInt32(((ListViewItem)sender).Uid) };

                    Utils.PutRequestScreensAsync(screen.ScreenNumber, screen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            string pickedFile;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    pickedFile = openFileDialog.FileName;

                    foreach (object i in Elements.Children)
                    {
                        if ((i as ListViewItem).IsSelected)
                        {
                            int screenNumber = Convert.ToInt32(((ListViewItem)i).Uid);
                            ScreenTemplateElement screen = new ScreenTemplateElement { ScreenNumber = screenNumber, IsPlaylist = false, Path = pickedFile };

                            Utils.PutRequestScreensAsync(screen.ScreenNumber, screen);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PickAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (object i in Elements.Children)
            {
                (i as ListViewItem).IsSelected = true;
            }
        }
        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ScreenTemplate template = await Utils.GetRequestScreensAsync();
            Utils.PostRequestScreensAsync(template);
        }
        private void Screen_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string pickedFile;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    int screenNumber = Convert.ToInt32(((ListViewItem)sender).Uid);
                    pickedFile = openFileDialog.FileName;
                    ScreenTemplateElement screen = new ScreenTemplateElement { ScreenNumber = screenNumber, Path = pickedFile, IsPlaylist = false };

                    Utils.PutRequestScreensAsync(screenNumber, screen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Sets the value of the <see cref="ScreenTemplate"/>
        /// </summary>
        /// <param name="screenTemplate">The element to set</param>
        public async void SetScreenTemplate(ScreenTemplate screenTemplate)
        {
            CurrentScreenTemplate = screenTemplate;
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!string.IsNullOrEmpty(CurrentScreenTemplate.ScreenTemplateElements[i].Path))
                    {
                        string exp = CurrentScreenTemplate.ScreenTemplateElements[i].Path.Split("\\").LastOrDefault().Split('.').LastOrDefault();
                        if (Utils.ImageExp.Contains(exp))
                        {
                            Image img = Utils.CreateImage(CurrentScreenTemplate.ScreenTemplateElements[i].Path);
                            (Elements.Children[i] as ListViewItem).Content = img;
                        }
                        else if (Utils.VideoExp.Contains(exp))
                        {
                            Canvas video = CreateVideoCanvas(i);
                            (Elements.Children[i] as ListViewItem).Content = video;
                        }
                        else if (exp == "gif")
                        {
                            MediaElement video = Utils.CreateVideo(CurrentScreenTemplate.ScreenTemplateElements[i].Path);
                            (Elements.Children[i] as ListViewItem).Content = video;
                        }

                    }
                }
                await UpdateHistory(screenTemplate.ScreenTemplateElements.FirstOrDefault());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Sets the value of the <see cref="ScreenTemplateElement"/> to a given screen number
        /// </summary>
        /// <param name="screenNumber">Screen number</param>
        /// <param name="element">The element to set</param>
        public async void SetScreenTemplateElement(int screenNumber, ScreenTemplateElement element)
        {
            if (element != null && element.Path != null)
            {
                CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1] = element;
                string exp = CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path
                    .Split("\\")
                    .LastOrDefault()
                    .Split('.')
                    .LastOrDefault();
                if (Utils.ImageExp.Contains(exp))
                {
                    Image img = Utils.CreateImage(CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path);
                    (Elements.Children[screenNumber - 1] as ListViewItem).Content = img;
                }
                else if (Utils.VideoExp.Contains(exp))
                {
                    Canvas video = CreateVideoCanvas(screenNumber - 1);
                    (Elements.Children[screenNumber - 1] as ListViewItem).Content = video;
                }
                else if (exp == "gif")
                {
                    MediaElement video = Utils.CreateVideo(CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1].Path);
                    (Elements.Children[screenNumber - 1] as ListViewItem).Content = video;
                }

                await UpdateHistory(element);
            }
        }

        public void SetOnline(bool[] onlineStatuses)
        {
            try
            {
                for (int i = 0; i < onlineStatuses.Length; i++)
                {
                    bool item = onlineStatuses[i];

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Ellipse ellipse = onlineStatusesGrid.Children[i] as Ellipse;
                        if (item)
                        {
                            ellipse.Fill = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            ellipse.Fill = new SolidColorBrush(Colors.Red);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Canvas CreateVideoCanvas(int screenNumber)
        {
            VideoDrawing videoDrawing = Utils.CreateVideoDrawing(CurrentScreenTemplate.ScreenTemplateElements[screenNumber].Path);
            DrawingBrush brush = new DrawingBrush(videoDrawing);

            Binding bindingHeight = new Binding("ActualHeight")
            {
                Source = Elements.Children[screenNumber] as ListViewItem
            };
            Binding bindingWidth = new Binding("ActualWidth")
            {
                Source = Elements.Children[screenNumber] as ListViewItem
            };

            Size videoSize = Utils.GetVideoSize(CurrentScreenTemplate.ScreenTemplateElements[screenNumber].Path);

            MultiBinding multiBindingWidth = new MultiBinding();
            multiBindingWidth.Bindings.Add(bindingWidth);
            multiBindingWidth.Bindings.Add(bindingHeight);
            multiBindingWidth.Converter = new VideoWidthConverter();
            multiBindingWidth.ConverterParameter = $"{videoSize.Width}, {videoSize.Height}";

            MultiBinding multiBindingHeight = new MultiBinding();
            multiBindingHeight.Bindings.Add(bindingWidth);
            multiBindingHeight.Bindings.Add(bindingHeight);
            multiBindingHeight.Converter = new VideoHeigthConverter();
            multiBindingHeight.ConverterParameter = $"{videoSize.Width}, {videoSize.Height}";


            Canvas canvas = new Canvas { Background = brush };

            canvas.SetBinding(Canvas.HeightProperty, multiBindingHeight);
            canvas.SetBinding(Canvas.WidthProperty, multiBindingWidth);
            return canvas;
        }

        private void DetermineScreenNumber_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            clickedScreenNumber = Convert.ToInt32((sender as ListViewItem).Uid);
        }

        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            string pickedFile;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    pickedFile = openFileDialog.FileName;
                    int screenNumber = clickedScreenNumber;

                    Utils.PostRequestBackgroundAsync(screenNumber, pickedFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task UpdateHistory(ScreenTemplateElement element)
        {
            using HistoryDbContext historyContext = new HistoryDbContext();
            await historyContext.History.AddAsync(new History
            {
                Id = Guid.NewGuid(),
                Changed = DateTime.Now,
                ScreenTemplate = new ScreenTemplate
                {
                    ScreenTemplateElements = GetCopy(CurrentScreenTemplate),
                    Title = element.Path
                }
            });

            await historyContext.SaveChangesAsync();
        }

        private List<ScreenTemplateElement> GetCopy(ScreenTemplate screenTemplate)
        {
            List<ScreenTemplateElement> screenTemplateElements = new List<ScreenTemplateElement>();
            foreach (var item in screenTemplate.ScreenTemplateElements)
            {
                screenTemplateElements.Add(new ScreenTemplateElement
                {
                    IsPlaylist = item.IsPlaylist,
                    Path = item.Path,
                    ScreenNumber = item.ScreenNumber
                });
            }

            return screenTemplateElements;
        }
    }
}