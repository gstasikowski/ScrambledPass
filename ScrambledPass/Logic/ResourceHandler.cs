using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace ScrambledPass.Logic
{
    public class ResourceHandler
    {
        private static void LoadResource(string filePath, string fileName)
        {
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(string.Format("pack://{0}{1}.xaml", filePath, fileName))
            };

            var existingDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                rd => rd.Source.OriginalString.StartsWith("pack://" + filePath));

            if (existingDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(existingDictionary);
            }

            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public void SwitchLanguage(string cultureCode)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureCode);
            LoadResource(Refs.dataBank.DefaultLanguagePath, cultureInfo.Name);
        }

        public void SwitchTheme(string newTheme)
        {
            LoadResource(Refs.dataBank.DefaultThemePath, newTheme);
        }
    }
}
