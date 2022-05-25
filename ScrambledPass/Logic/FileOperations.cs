using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ScrambledPass.Logic
{
    public static class FileOperations
    {
        public static List<string> LoadDefaultWordList()
        {
            List<string> wordList = new List<string>();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(Refs.dataBank.DefaultWordListFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string newWord = "";

                    do
                    {
                        newWord = reader.ReadLine();
                        wordList.Add(newWord);
                    } while (!string.IsNullOrEmpty(newWord));

                    reader.Close();
                }
            }
            catch
            { new ErrorHandler("ErrorFileNotFound"); }
            
            Refs.dataBank.SetSetting("lastWordList", string.Empty);
            return wordList;
        }

        public static List<string> LoadCustomWordList(string filePath)
        {
            List<string> wordList = new List<string>();

            if (File.Exists(filePath))
            {
                wordList.Clear();
                wordList.AddRange(File.ReadAllLines(filePath));

                Refs.dataBank.SetSetting("lastWordList", filePath);
            }
            else
            { new ErrorHandler("ErrorFileNotFound"); }

            return wordList;
        }

        public static void LoadSettings()
        {
            string configFilePath = Refs.dataBank.DefaultConfigFile;

            if (File.Exists(configFilePath))
            {
                string configFile = File.ReadAllText(configFilePath);
                XElement rootElement = XElement.Parse(configFile);

                foreach (var element in rootElement.Elements())
                { Refs.dataBank.SetSetting(element.Name.LocalName, element.Value); }
            }
            else
            {
                Refs.dataBank.DefaultSettings();
                SaveSettings();
            }

            Refs.resourceHandler.SwitchLanguage(Refs.dataBank.GetSetting("languageID"));
        }

        public static void SaveSettings()
        {
            Dictionary<string, string> appSettings = Refs.dataBank.GetAllSettings();

            FileStream fileStream;
            fileStream = new FileStream(Refs.dataBank.DefaultConfigFile, FileMode.Create);

            XElement rootElement = new XElement("Config", appSettings.Select(kv => new XElement(kv.Key, kv.Value)));
            XmlSerializer serializer = new XmlSerializer(rootElement.GetType());
            serializer.Serialize(fileStream, rootElement);

            fileStream.Close();
        }

        public static void LoadTranslations()
        {
            foreach (string filePath in Directory.EnumerateFiles(Refs.dataBank.DefaultLanguagePath))
            {
                string cultureCode = filePath.Substring(filePath.LastIndexOf('\\') + 1).Replace(".xaml", "");
                var newCulture = System.Globalization.CultureInfo.GetCultureInfo(cultureCode);
                Refs.dataBank.AddAvailableLanguage(string.Format("{0} [{1}]", newCulture.DisplayName, newCulture.Name));
            }
        }

        public static void LoadThemes()
        {
            foreach (string filePath in Directory.EnumerateFiles(Refs.dataBank.DefaultThemePath))
            {
                string themeName = filePath.Substring(filePath.LastIndexOf('\\') + 1).Replace(".xaml", "");
                Refs.dataBank.AddAvailableTheme(themeName);
            }
        }
    }
}
