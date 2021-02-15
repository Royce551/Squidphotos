using Squidphotos.Styles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Squidphotos
{
    public enum Skin
    {
        Light, Dark
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Skin CurrentSkin { get; set; } = Skin.Dark;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ChangeSkin(Skin.Dark);
        }
        public void ChangeSkin(Skin newSkin)
        {
            CurrentSkin = newSkin;

            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {

                if (dict is SkinResourceDictionary skinDict)
                    skinDict.UpdateSource();
                else
                    dict.Source = dict.Source;
            }
        }
    }
}
