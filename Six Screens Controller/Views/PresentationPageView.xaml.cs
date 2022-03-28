using NetOffice.PowerPointApi;
using Six_Screens_Controller.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Six_Screens_Controller.Views
{
    /// <summary>
    /// Interaction logic for PresentationPageView.xaml
    /// </summary>
    public partial class PresentationPageView : UserControl
    {
        private string _currentPresentation;
        private int _currentSlide;
        public PresentationPageView()
        {
            InitializeComponent();
            Loaded += PresentationPageView_Loaded;
        }

        public PresentationPageView(string presentation, int slideNumber)
        {
            InitializeComponent();
            Loaded += PresentationPageView_Loaded;
            _currentPresentation = presentation;
            ChangeSlide(slideNumber);
        }
        private void PresentationPageView_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentSlide.Source == null)
            {
                Image defaultImg = Utils.CreateImage($"{Directory.GetCurrentDirectory()}/assets/Default.jpg");
                CurrentSlide.Source = defaultImg.Source;
            }
        }

        private void PreviosSlide_Click(object sender, RoutedEventArgs e)
        {
            ChangeSlide(_currentSlide - 1);
        }

        private void NextSlide_Click(object sender, RoutedEventArgs e)
        {
            ChangeSlide(_currentSlide + 1);
        }

        private void CurrentSlide_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (Directory.Exists(_currentPresentation))
                    {
                        Directory.Delete(_currentPresentation);
                    }
                    string presentationsPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                    SetPresentation(presentationsPath);
                    ChangeSlide(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetPresentation(string presentationsPath)
        {
            string currentFolder = Directory.GetCurrentDirectory();
            if (!Directory.GetDirectories($"{currentFolder}").Contains($"{currentFolder}\\Presentation"))
            {
                Directory.CreateDirectory($"{currentFolder}\\Presentation");
            }

            _currentPresentation = $"{currentFolder}\\Presentation\\{presentationsPath.Split('\\').Last().Split('.')[0]}";
            ConvertPresentationToImage(presentationsPath, currentFolder);
        }

        private void ConvertPresentationToImage(string presentationsPath, string currentFolder)
        {
            NetOffice.PowerPointApi.Application pptApplication = new NetOffice.PowerPointApi.Application();
            Presentation pptPresentation = pptApplication.Presentations.Open(presentationsPath, false, false, false);
            if (!Directory.GetDirectories($"{currentFolder}\\Presentation").Contains(_currentPresentation))
            {
                Directory.CreateDirectory($"{_currentPresentation}");
                for (int i = 1; i <= pptPresentation.Slides.Count; i++)
                    pptPresentation.Slides[i].Export($"{_currentPresentation}\\{i}.png", "png", 1920, 1080);
            }
            pptPresentation.Close();
        }

        private void ChangeSlide(int number)
        {
            try
            {
                string newSlide = $"{_currentPresentation}\\{number}.png";
                if (File.Exists(newSlide))
                {
                    _currentSlide = number;
                    Image slide = Utils.CreateImage(newSlide);
                    CurrentSlide.Source = slide.Source;
                    ScreenTemplate screenTemplate = new ScreenTemplate()
                    {
                        ScreenTemplateElements =
                    {
                        new ScreenTemplateElement(){Path = newSlide, IsPlaylist = false, ScreenNumber = 1},
                        new ScreenTemplateElement(){Path = newSlide, IsPlaylist = false, ScreenNumber = 2},
                        new ScreenTemplateElement(){Path = newSlide, IsPlaylist = false, ScreenNumber = 3},
                        new ScreenTemplateElement(){Path = newSlide, IsPlaylist = false, ScreenNumber = 4},
                        new ScreenTemplateElement(){Path = newSlide, IsPlaylist = false, ScreenNumber = 5},
                        new ScreenTemplateElement(){Path = newSlide, IsPlaylist = false, ScreenNumber = 6},
                    }
                    };
                    Utils.PostRequestScreensAsync(screenTemplate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
