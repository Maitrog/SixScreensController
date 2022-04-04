using NetOffice.PowerPointApi;
using Newtonsoft.Json;
using Six_Screens_Controller.Models;
using System;
using System.Collections.Generic;
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
        private Dictionary<string, long> _existingPresentations;

        static private readonly string _presentationsFolderPath = $"{Directory.GetCurrentDirectory()}\\Presentations";
        static private readonly string _presentationsJsonPath = $"{_presentationsFolderPath}\\presentations.json";
        public PresentationPageView()
        {
            InitializeComponent();
            Loaded += PresentationPageView_Loaded;
            CreatePresentationsFolder();
        }

        public PresentationPageView(string presentation, int slideNumber)
        {
            InitializeComponent();
            Loaded += PresentationPageView_Loaded;
            CreatePresentationsFolder();
            _currentPresentation = presentation;
            ChangeSlide(slideNumber);
        }
        private void PresentationPageView_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentSlide.Source == null)
            {
                SetDefaultImage();
            }
            _existingPresentations = JsonConvert.DeserializeObject<Dictionary<string, long>>(File.ReadAllText(_presentationsJsonPath));
            if (_existingPresentations == null)
            {
                _existingPresentations = new Dictionary<string, long>();
            }
        }

        private void SetDefaultImage()
        {
            Image defaultImg = Utils.CreateImage($"{Directory.GetCurrentDirectory()}/assets/Default.jpg");
            CurrentSlide.Source = defaultImg.Source;
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
                    SetDefaultImage();

                    ScreenTemplate screenTemplate = new ScreenTemplate()
                    {
                        ScreenTemplateElements =
                        {
                            new ScreenTemplateElement(){Path = $"{Directory.GetCurrentDirectory()}/assets/Default.jpg", IsPlaylist = false, ScreenNumber = 1},
                            new ScreenTemplateElement(){Path = $"{Directory.GetCurrentDirectory()}/assets/Default.jpg", IsPlaylist = false, ScreenNumber = 2},
                            new ScreenTemplateElement(){Path = $"{Directory.GetCurrentDirectory()}/assets/Default.jpg", IsPlaylist = false, ScreenNumber = 3},
                            new ScreenTemplateElement(){Path = $"{Directory.GetCurrentDirectory()}/assets/Default.jpg", IsPlaylist = false, ScreenNumber = 4},
                            new ScreenTemplateElement(){Path = $"{Directory.GetCurrentDirectory()}/assets/Default.jpg", IsPlaylist = false, ScreenNumber = 5},
                            new ScreenTemplateElement(){Path = $"{Directory.GetCurrentDirectory()}/assets/Default.jpg", IsPlaylist = false, ScreenNumber = 6},
                        }
                    };
                    Utils.PostRequestScreensAsync(screenTemplate);

                    string presentationsPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                    SetPresentation(presentationsPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetPresentation(string presentationsPath)
        {
            _currentPresentation = $"{_presentationsFolderPath}\\{presentationsPath.Split('\\').Last().Split('.')[0]}";
            ConvertPresentationToImage(presentationsPath);
            ChangeSlide(1);
        }

        private void ConvertPresentationToImage(string presentationsPath)
        {
            FileInfo presentationInfo = new FileInfo(presentationsPath);
            if (IsPresentationExists(presentationInfo))
            {
                return;
            }

            NetOffice.PowerPointApi.Application pptApplication = new NetOffice.PowerPointApi.Application();
            Presentation pptPresentation = pptApplication.Presentations.Open(presentationsPath, false, false, false);

            if (!Directory.GetDirectories($"{_presentationsFolderPath}\\Presentations").Contains(_currentPresentation))
            {
                Directory.CreateDirectory($"{_currentPresentation}");
                for (int i = 1; i <= pptPresentation.Slides.Count; i++)
                {
                    pptPresentation.Slides[i].Export($"{_currentPresentation}\\{i}.png", "png", 1920, 1080);
                }
                
                _existingPresentations.Add(presentationInfo.Name, presentationInfo.Length);
                File.WriteAllText(_presentationsJsonPath, JsonConvert.SerializeObject(_existingPresentations));
            }
            pptPresentation.Close();
            pptApplication.Quit();
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
        private static void CreatePresentationsFolder()
        {
            if (!Directory.Exists(_presentationsFolderPath))
            {
                Directory.CreateDirectory(_presentationsFolderPath);
            }
            if (!File.Exists(_presentationsJsonPath))
            {
                File.Create(_presentationsJsonPath);
            }
        }

        private bool IsPresentationExists(FileInfo presentationInfo)
        {
            if (_existingPresentations.TryGetValue(presentationInfo.Name, out long length))
            {
                if (length != presentationInfo.Length)
                {
                    GC.Collect(GC.GetGeneration(CurrentSlide));
                    GC.WaitForPendingFinalizers();

                    if (Directory.Exists(_currentPresentation))
                    {
                        Directory.Delete(_currentPresentation, true);
                    }

                    _existingPresentations.Remove(presentationInfo.Name);
                    File.WriteAllText(_presentationsJsonPath, JsonConvert.SerializeObject(_existingPresentations));
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
