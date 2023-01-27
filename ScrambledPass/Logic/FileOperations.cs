using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ScrambledPass.Logic
{
    public static class FileOperations
    {
        public static void LoadResources()
        {
            LoadSettings();
            PrepareWordList();
        }

        public static void PrepareWordList()
        {
            string wordlistFilePath = Refs.dataBank.GetSetting("lastWordList");

            Refs.dataBank.WordList.Clear();
            
            if (wordlistFilePath == string.Empty)
            {
                Refs.dataBank.WordList.AddRange(FileOperations.LoadDefaultWordList());
            }
            else
            {
                Refs.dataBank.WordList.AddRange(FileOperations.LoadCustomWordList(wordlistFilePath));
            }
        }

        public static List<string> LoadDefaultWordList()
        {
            List<string> wordList = new List<string>();
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(Refs.dataBank.DefaultWordListFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string? newWord = reader.ReadLine();
                    
                    while (!string.IsNullOrEmpty(newWord))
                    {
                        wordList.Add(newWord);
                        newWord = reader.ReadLine();
                    }
                    
                    reader.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                new ErrorHandler("ErrorFileNotFound", null, e.InnerException);
            }
            
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
            {
                new ErrorHandler("ErrorFileNotFound");
            }

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
                {
                    Refs.dataBank.SetSetting(element.Name.LocalName, element.Value);
                }
            }
            else
            {
                Refs.dataBank.DefaultSettings();
                SaveSettings();
            }
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
    }
}
