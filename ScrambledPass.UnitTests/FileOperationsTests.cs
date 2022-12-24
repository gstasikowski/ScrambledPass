using ScrambledPass.Logic;

namespace ScrambledPass.UnitTests
{
    public class FileOperationsTests
    {    
        private readonly string _customWordFilePath = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("/bin/")), "/Data/testFile.txt");

        private void SetupWordlist(string wordlistPath)
        {
            Refs.dataBank.SetSetting(key: "lastWordList", value: wordlistPath);
            Logic.FileOperations.PrepareWordList();
        }
        
        [Fact]
        public void Should_load_default_wordlist()
        {
            SetupWordlist(string.Empty);

            Assert.True(Refs.dataBank.WordList.Count() > 0);
        }

        [Fact]
        public void Should_load_custom_wordlist()
        {
            SetupWordlist(_customWordFilePath);
            
            Assert.True(Refs.dataBank.WordList.Count() > 0);
        }
    }
}