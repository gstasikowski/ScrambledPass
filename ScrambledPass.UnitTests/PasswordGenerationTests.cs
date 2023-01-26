using ScrambledPass.Logic;

namespace ScrambledPass.UnitTests
{
    public class PasswordGenerationTests
    {
        private const int SymbolMode = 2;
        private const int SymbolCount = 10;

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
            bool useCapitalLetters,
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
                        useCapitalLetters,
                        useNumbers,
                        useSymbols
                    );

            Assert.True(
                newPassword.Split(' ').Count() == wordCount 
                && !PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_letter_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        wordCount: 0,
                        symbolMode: SymbolMode,
                        symbolCount: SymbolCount,
                        randomCharacterSize: false,
                        useLetters: true,
                        useCapitalLetters: false,
                        useNumbers: false,
                        useSymbols: false
                    );

            Assert.True(
                PasswordContainsLetters(newPassword)
                && !PasswordContainsCapitalLetters(newPassword)
                && !PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_capital_letter_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        wordCount: 0,
                        symbolMode: SymbolMode,
                        symbolCount: SymbolCount,
                        randomCharacterSize: false,
                        useLetters: false,
                        useCapitalLetters: true,
                        useNumbers: false,
                        useSymbols: false
                    );

            Assert.True(
                !PasswordContainsLetters(newPassword)
                && PasswordContainsCapitalLetters(newPassword)
                && !PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_number_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        wordCount: 0,
                        symbolMode: SymbolMode,
                        symbolCount: SymbolCount,
                        randomCharacterSize: false,
                        useLetters: false,
                        useCapitalLetters: false,
                        useNumbers: true,
                        useSymbols: false
                    );

            Assert.True(
                !PasswordContainsLetters(newPassword)
                && !PasswordContainsCapitalLetters(newPassword)
                && PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_symbol_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        wordCount: 0,
                        symbolMode: SymbolMode,
                        symbolCount: SymbolCount,
                        randomCharacterSize: false,
                        useLetters: false,
                        useCapitalLetters: false,
                        useNumbers: false,
                        useSymbols: true
                    );

            Assert.True(
                !PasswordContainsLetters(newPassword)
                && !PasswordContainsCapitalLetters(newPassword)
                && !PasswordContainsNumbers(newPassword)
                && PasswordContainsSymbols(newPassword)
            );
        }

        private bool PasswordContainsLetters(string password)
        {
            for (int i = 0; i < 26; i++)
            {
                if (password.Contains((char)('a' + i)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool PasswordContainsCapitalLetters(string password)
        {
            for (int i = 0; i < 26; i++)
            {
                if (password.Contains((char)('A' + i)))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool PasswordContainsNumbers(string password)
        {
            for (int number = 0; number < 10; number++)
            {
                if (password.Contains(number.ToString()))
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool PasswordContainsSymbols(string password)
        {
            foreach (char symbol in Refs.dataBank.Symbols)
            {
                if (symbol == ' ' || symbol == '.' || symbol == '&' || symbol == '\'')
                {
                    continue;
                }

                if (password.Contains(symbol))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}