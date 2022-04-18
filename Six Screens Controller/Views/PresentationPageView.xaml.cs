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

        static private readonly string _presentationsFolderPath = $"{Directory.GetCurrentDirectory()}\\Presentations";
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

                    ScreenTemplate screenTemplate = new ScreenTemplate($"{Directory.GetCurrentDirectory()}/assets/Default.jpg");
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
            PresentationInfo presentationInfo = new PresentationInfo(presentationsPath);
            using (PresentationContext context = new PresentationContext())
            {
                PresentationInfo fromDb = context.Presentations.Where(x => x.Name == presentationInfo.Name).FirstOrDefault();
                if (fromDb == null || fromDb.HashSum != presentationInfo.HashSum)
                {
                    _currentPresentation = $"{_presentationsFolderPath}\\{context.Presentations.Count()}";
                    context.Add(presentationInfo);
                    context.SaveChanges();
                    ConvertPresentationToImage(presentationsPath);
                }
                else
                {
                    _currentPresentation = $"{_presentationsFolderPath}\\{fromDb.Id}";
                }
            }
            ChangeSlide(1);
        }

        private void ConvertPresentationToImage(string presentationsPath)
        {
            NetOffice.PowerPointApi.Application pptApplication = new NetOffice.PowerPointApi.Application();
            Presentation pptPresentation = pptApplication.Presentations.Open(presentationsPath, false, false, false);

            if (!Directory.GetDirectories($"{_presentationsFolderPath}").Contains(_currentPresentation))
            {
                Directory.CreateDirectory($"{_currentPresentation}");
                for (int i = 1; i <= pptPresentation.Slides.Count; i++)
                {
                    pptPresentation.Slides[i].Export($"{_currentPresentation}\\{i}.png", "png", 1920, 1080);
                }
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
                    ScreenTemplate screenTemplate = new ScreenTemplate(newSlide);
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
        }
    }
}
