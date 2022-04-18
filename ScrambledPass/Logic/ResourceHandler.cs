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
            var dict = new ResourceDictionary
            {
                Source = new Uri(string.Format("pack://{0}{1}.xaml", filePath, fileName))
            };

            var existingDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                rd => rd.Source.OriginalString.StartsWith("pack://" + filePath));

            if (existingDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(existingDict);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
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
