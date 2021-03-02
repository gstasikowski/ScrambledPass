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
            LoadDefaultWordList();
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

            return wordList;
        }

        public List<string> LoadCustomWordList(string filePath)
        {
            List<string> wordList = new List<string>();

            if (File.Exists(filePath))
            {
                wordList.Clear();
                wordList.AddRange(File.ReadAllLines(filePath));
            }
            else
            { new ErrorHandler("fileNotFound"); }

            return wordList;
        }
    }
}
