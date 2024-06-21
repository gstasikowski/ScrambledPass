using ScrambledPass.Logic;

// Need to stub file loading so tests don't break each other by using the real database.
namespace ScrambledPass.UnitTests
{
    public class FileOperationsTests
    {
        private static string _defaultWordListPath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(".UnitTests/")), "defaultWordList.txt");
        private static string _customWordFilePath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("/bin/")), "Data/testFile.txt");

        private Core _core;

        public FileOperationsTests()
        {
            _core = new Core();
            _core.dataBank.SetSetting(key: "lastWordList", value: string.Empty);
            _core.fileOperations.PrepareWordList();
        }

        private void SetupWordlist(string wordlistPath)
        {
            _core.dataBank.SetSetting(key: "lastWordList", value: wordlistPath);
            _core.fileOperations.PrepareWordList();
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
            int wordListCount = _core.dataBank.WordList.Count;
            int lineCount = GetWordListLineCount(_defaultWordListPath);

            Assert.True(wordListCount == lineCount);
        }

        [Fact]
        public void Should_load_custom_wordlist()
        {
            SetupWordlist(_customWordFilePath);
            int wordListCount = _core.dataBank.WordList.Count;
            int lineCount = GetWordListLineCount(_customWordFilePath);

            Assert.True(wordListCount == lineCount);
        }
    }
}