using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

        public void DefaultSettings()
        {
            Properties.Settings.Default["languageID"] = "0";
            Properties.Settings.Default["lastWordList"] = string.Empty;
            Properties.Settings.Default["rememberLastWordList"] = "False";
            Properties.Settings.Default["defWordCount"] = "5";
            Properties.Settings.Default["defCharCount"] = "5";
            Properties.Settings.Default.Save();
        }
    }
}
