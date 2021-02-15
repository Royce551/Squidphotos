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
using System.Windows.Shapes;

namespace Squidphotos.Pages
{
    /// <summary>
    /// Interaction logic for ViewerPage.xaml
    /// </summary>
    public partial class ViewerPage : Page
    {
        private string filePath;
        private MemoryStream image;
        public ViewerPage(string filePath)
        {
            this.filePath = filePath;
            InitializeComponent();
            PresentImage(filePath);
        }
        public async void PresentImage(string filePath)
        {
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
    }
}
