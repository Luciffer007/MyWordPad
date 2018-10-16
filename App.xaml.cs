using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Globalization;

namespace MyWordPad
{
    public partial class App : Application
    {
        // List of supported languages
        private static List<CultureInfo> languagesList = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return languagesList;
            }
        }

        // Current language
        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                // Check the selection of the current application language
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                // Change the current application language
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                // Creating a ResourceDictionary of the new language and connecting the corresponding language resources
                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri("Resources/lang.ru-RU.xaml", UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                // Deleting a ResourceDictionary of a old language and adding ResourceDictionary of a new language
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }
            }
        }

        public App()
        {
            // Filling the list of supported languages
            languagesList.Clear();
            languagesList.Add(new CultureInfo("en-US"));
            languagesList.Add(new CultureInfo("ru-RU"));

            Startup += AppStart;
            Exit += AppExit;
        }

        // Loading saved language option
        static void AppStart(object sender, StartupEventArgs e)
        {
            Language = MyWordPad.Properties.Settings.Default.DefaultLanguage;
        }

        // Setting and saving user selected language option
        static void AppExit(object sender, ExitEventArgs e)
        {
            MyWordPad.Properties.Settings.Default.DefaultLanguage = Language;
            MyWordPad.Properties.Settings.Default.Save();
        }
    }
}
