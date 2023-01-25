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
                        0,
                        SymbolMode,
                        SymbolCount,
                        false,
                        true,
                        false,
                        false,
                        false
                    );

            Assert.True(PasswordContainsLetters(newPassword)
                && !PasswordContainsBigLetters(newPassword)
                && !PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_capital_letter_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        0,
                        SymbolMode,
                        SymbolCount,
                        false,
                        false,
                        true,
                        false,
                        false
                    );

            Assert.True(!PasswordContainsLetters(newPassword)
                && PasswordContainsBigLetters(newPassword)
                && !PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_number_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        0,
                        SymbolMode,
                        SymbolCount,
                        false,
                        false,
                        false,
                        true,
                        false
                    );

            Assert.True(!PasswordContainsLetters(newPassword)
                && !PasswordContainsBigLetters(newPassword)
                && PasswordContainsNumbers(newPassword)
                && !PasswordContainsSymbols(newPassword)
            );
        }

        [Fact]
        public void Should_generate_symbol_only_password()
        {
            string newPassword = Refs.generator.GeneratePassword(
                        0,
                        SymbolMode,
                        SymbolCount,
                        false,
                        false,
                        false,
                        false,
                        true
                    );

            Assert.True(!PasswordContainsLetters(newPassword)
                && !PasswordContainsBigLetters(newPassword)
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

        private bool PasswordContainsBigLetters(string password)
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