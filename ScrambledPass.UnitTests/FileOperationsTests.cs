using ScrambledPass.Logic;

namespace ScrambledPass.UnitTests
{
    public class FileOperationsTests
    {
        private static string _defaultWordListPath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(".UnitTests/")), "defaultWordList.txt");
        private static string _customWordFilePath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("/bin/")), "Data/testFile.txt");

        private void SetupWordlist(string wordlistPath)
        {
            Refs.dataBank.SetSetting(key: "lastWordList", value: wordlistPath);
            Logic.FileOperations.PrepareWordList();
        }

        private int GetWordListLineCount(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    List<string> wordList = new List<string>(File.ReadAllLines(filePath));
                    return wordList.Count;
                }
            }
            catch (FileNotFoundException e)
            {
                new ErrorHandler("ErrorFileNotFound", null, e.InnerException);
            }

            return -1;
        }
        
        [Fact]
        public void Should_load_default_wordlist()
        {
            SetupWordlist(string.Empty);
            int lineCount = GetWordListLineCount(_defaultWordListPath);

            Assert.True(Refs.dataBank.WordList.Count == lineCount);
        }

        [Fact]
        public void Should_load_custom_wordlist()
        {
            SetupWordlist(_customWordFilePath);
            int lineCount = GetWordListLineCount(_customWordFilePath);
            
            Assert.True(Refs.dataBank.WordList.Count() == lineCount);
        }
    }
}