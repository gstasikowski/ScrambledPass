using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace ScrambledPass.Logic
{
    public class FileOperations
    {
        string defaultNamespace = "ScrambledPass";
        string defaultWordList = "defaultWordList.txt";

        public FileOperations()
        {

        }

        public List<string> LoadDefaultWordList()
        {
            List<string> wordList = new List<string>();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = defaultNamespace + ".Resources." + defaultWordList;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
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

            SaveSettings("lastWordList", string.Empty);
            return wordList;
        }

        public List<string> LoadCustomWordList(string filePath)
        {
            List<string> wordList = new List<string>();

            if (File.Exists(filePath))
            {
                wordList.Clear();
                wordList.AddRange(File.ReadAllLines(filePath));

                SaveSettings("lastWordList", filePath);
            }
            else
            { new ErrorHandler("fileNotFound"); }

            return wordList;
        }

        public void SaveSettings(string property, string value)
        {
            Properties.Settings.Default[property] = value;
            Properties.Settings.Default.Save();
        }

        public void LoadTranslations()
        {
            foreach (string fileName in Directory.EnumerateFiles(System.AppDomain.CurrentDomain.BaseDirectory + "Translation"))
            {
                string translationFile = string.Empty;

                if (File.Exists(fileName))
                {
                    translationFile = File.ReadAllText(fileName);
                }
                else
                { new ErrorHandler("fileNotFound"); }

                XElement rootElement = XElement.Parse(translationFile);
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var el in rootElement.Elements())
                {
                    dict.Add(el.Name.LocalName, el.Value);
                }

                App app = (App)App.Current;
                app.dataBank.AddAvailableLanguage(fileName.Substring(fileName.IndexOf('_') - 2, 2));
                app.dataBank.FillLanguageDictionary(fileName.Substring(fileName.IndexOf('_') + 1).Replace(".xml", ""), dict);
            }
        }
    }
}
