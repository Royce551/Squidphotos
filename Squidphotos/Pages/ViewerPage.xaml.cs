using Microsoft.Win32;
using Squidphotos.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Squidphotos.Pages
{
    /// <summary>
    /// Interaction logic for ViewerPage.xaml
    /// </summary>
    public partial class ViewerPage : Page
    {
        public readonly string[] AllowableFileTypes = {".bmp", ".jpg", ".jpeg", ".png", ".tiff", ".gif", ".ico"};

        private string filePath;
        private List<string> imagesInDirectory = new List<string>();
        private int currentImage = 0;
        private MemoryStream image;
        public ViewerPage(string filePath)
        {
            this.filePath = filePath;
            InitializeComponent();
            Init();
            ContentBorder.Interacted += ContentBorder_Interacted;
        }
        public async void Init()
        {
            PositionTextBlock.Text = "Loading, please wait...";
            await Task.Run(() => FindImagesInDirectory(filePath));
            PresentImage();
        }
        public async void PresentImage()
        {       
            if (!AllowableFileTypes.Contains(Path.GetExtension(imagesInDirectory[currentImage])))
            {
                MessageBox.Show("That file type can't be displayed :(");
                return;
            }
            var image = new MemoryStream(await Task.Run(() => File.ReadAllBytes(imagesInDirectory[currentImage])));
            PositionTextBlock.Text = $"{currentImage + 1}/{imagesInDirectory.Count}";
            BackgroundImage.Source = ForegroundImage.Source = BitmapFrame.Create(image, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            this.image = image;
        }
        public void FindImagesInDirectory(string filePath)
        {
            imagesInDirectory.Clear();
            imagesInDirectory.AddRange(Directory.EnumerateFiles(Path.GetDirectoryName(filePath), "*.*", SearchOption.TopDirectoryOnly).Where(x => AllowableFileTypes.Contains(Path.GetExtension(x))));
            currentImage = imagesInDirectory.FindIndex(y => y == filePath);
        }
        private void Page_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var margin = ContentBorder.Margin;
            var scrollValue = (e.Delta / 2.5) * -1;
            margin.Bottom += scrollValue;
            margin.Top += scrollValue;
            margin.Left += scrollValue;
            margin.Right += scrollValue;
            ContentBorder.Margin = margin;
        }
        private void ContentBorder_Interacted(object sender, EventArgs e)
        {
            ResetButton.Visibility = Visibility.Visible;
        }
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                Init();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ContentBorder.Reset();
            ResetButton.Visibility = Visibility.Collapsed;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage > 0)
            {
                currentImage--;
                PresentImage();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage < imagesInDirectory.Count - 1)
            {
                currentImage++;
                PresentImage();
            }
        }

        private void PositionTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var box = new TextEntryBox("What image do you want to jump to?", (currentImage + 1).ToString());
            box.ShowDialog();
            if (box.OK)
            {
                if (int.TryParse(box.Response, out int result))
                {
                    currentImage = result - 1;
                    PresentImage();
                }
                else MessageBox.Show("That's not a valid image.");
            }
        }
    }
}
