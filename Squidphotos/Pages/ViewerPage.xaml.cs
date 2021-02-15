using Microsoft.Win32;
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
        private MemoryStream image;
        public ViewerPage(string filePath)
        {
            this.filePath = filePath;
            InitializeComponent();
            PresentImage(filePath);
            ContentBorder.Interacted += ContentBorder_Interacted;
        }
        public async void PresentImage(string filePath)
        {
            if (!AllowableFileTypes.Contains(Path.GetExtension(filePath)))
            {
                MessageBox.Show("That file type can't be displayed :(");
                return;
            }
            var image = new MemoryStream(await Task.Run(() => File.ReadAllBytes(filePath)));
            BackgroundImage.Source = ForegroundImage.Source = BitmapFrame.Create(image, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            this.image = image;
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
            if (openFileDialog.ShowDialog() == true) PresentImage(openFileDialog.FileName);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ContentBorder.Reset();
            ResetButton.Visibility = Visibility.Collapsed;
        }
    }
}
