using ScrambledPass.Logic;

// Need to stub file loading so tests don't break each other by using the real database.
namespace ScrambledPass.UnitTests
{
    public class FileOperationsTests
    {
        // private static string _defaultWordListPath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(".UnitTests/")), "defaultWordList.txt");
        // private static string _customWordFilePath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("/bin/")), "Data/testFile.txt");

        // private void SetupWordlist(string wordlistPath)
        // {
        //     Refs.dataBank.SetSetting(key: "lastWordList", value: wordlistPath);
        //     Logic.FileOperations.PrepareWordList();
        // }

        // private int GetWordListLineCount(string filePath)
        // {
        //     try
        //     {
        //         if (File.Exists(filePath))
        //         {
        //             List<string> wordList = new List<string>(File.ReadAllLines(filePath));
        //             return wordList.Count;
        //         }
        //     }
        //     catch (FileNotFoundException e)
        //     {
        //         new ErrorHandler("ErrorFileNotFound", null, e.InnerException);
        //     }

        //     return -1;
        // }
        
        // [Fact]
        // public void Should_load_default_wordlist()
        // {
        //     SetupWordlist(string.Empty);
        //     int wordListCount = Refs.dataBank.WordList.Count;
        //     int lineCount = GetWordListLineCount(_defaultWordListPath);
        //     System.Console.WriteLine($"def list: {wordListCount}, compare: {lineCount}");
        //     Assert.True(wordListCount == lineCount);
        // }

        // [Fact]
        // public void Should_load_custom_wordlist()
        // {
        //     SetupWordlist(_customWordFilePath);
        //     int wordListCount = Refs.dataBank.WordList.Count;
        //     int lineCount = GetWordListLineCount(_customWordFilePath);
        //     System.Console.WriteLine($"def list: {wordListCount}, compare: {lineCount}");
        //     Assert.True(wordListCount == lineCount);
        // }
    }
}