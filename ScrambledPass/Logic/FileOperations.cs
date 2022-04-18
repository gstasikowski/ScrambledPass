using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ScrambledPass.Logic
{
    public class FileOperations
    {
        public FileOperations()
        {

        }

        public List<string> LoadDefaultWordList()
        {
            List<string> wordList = new List<string>();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(Refs.dataBank.DefaultWordList))
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

        public List<string> LoadCustomWordList(string filePath)
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

        public void LoadSettings()
        {
            string configFilePath = Refs.dataBank.DefaultConfigPath + "Config.xml";

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

        public void SaveSettings()
        {
            Dictionary<string, string> appSettings = Refs.dataBank.GetAllSettings();

            FileStream fileStream;
            fileStream = new FileStream(Refs.dataBank.DefaultConfigPath + "Config.xml", FileMode.Create);

            XElement rootElement = new XElement("Config", appSettings.Select(kv => new XElement(kv.Key, kv.Value)));
            XmlSerializer serializer = new XmlSerializer(rootElement.GetType());
            serializer.Serialize(fileStream, rootElement);

            fileStream.Close();
        }

        public void LoadTranslations()
        {
            foreach (string filePath in Directory.EnumerateFiles(Refs.dataBank.DefaultLanguagePath))
            {
                string cultureCode = filePath.Substring(filePath.LastIndexOf('\\') + 1).Replace(".xaml", "");
                var tempCulture = System.Globalization.CultureInfo.GetCultureInfo(cultureCode);
                Refs.dataBank.AddAvailableLanguage(string.Format("{0} [{1}]", tempCulture.DisplayName, tempCulture.Name));
            }
        }

        public void LoadThemes()
        {
            foreach (string filePath in Directory.EnumerateFiles(Refs.dataBank.DefaultThemePath))
            {
                string themeName = filePath.Substring(filePath.LastIndexOf('\\') + 1).Replace(".xaml", "");
                Refs.dataBank.AddAvailableTheme(themeName);
            }
        }
    }
}
