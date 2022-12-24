using ScrambledPass.Logic;

namespace ScrambledPass.UnitTests
{
    public class PasswordGenerationTests
    {        
        public PasswordGenerationTests()
        {
            Refs.dataBank.SetSetting(key: "lastWordList", value: string.Empty);
            Logic.FileOperations.PrepareWordList();
        }

        [Theory]
        [InlineData(2, 0, 0, false, false, false, false, false)]
        [InlineData(5, 0, 0, false, false, false, false, false)]
        [InlineData(10, 0, 0, false, false, false, false, false)]
        public void Should_generate_word_only_password(
            int wordCount,
            int symbolMode,
            int symbolCount,
            bool randomizeLetterSize,
            bool useLetters,
            bool useBigLetters,
            bool useNumbers,
            bool useSymbols
        )
        {
            string newPassword = Refs.generator.GeneratePassword(
                        wordCount,
                        symbolMode,
                        symbolCount,
                        randomizeLetterSize,
                        useLetters,
                        useBigLetters,
                        useNumbers,
                        useSymbols
                    );

            Assert.True(newPassword.Split(' ').Count() == wordCount);
        }
    }
}