using ScrambledPass.Helpers;

namespace ScrambledPass.Logic
{
    public class Generator
    {
        private Random _randomIndex = new Random();
        private List<char> _availableCharacters = new List<char>();

        public Generator()
        {}

        #region Methods (public)
        public string GeneratePassword(
            int wordCount,
            int symbolMode,
            int symbolCount,
            bool randomCharSize,
            bool useLetters,
            bool useBigLetters,
            bool useNumbers,
            bool useSymbols
        )
        {
            string newPassword = "";

            PrepareCharacterList(
                wordCount,
                useLetters,
                useBigLetters,
                useNumbers,
                useSymbols
            );

            InsertWords(wordCount, ref newPassword);

            if (randomCharSize)
            {
                newPassword = RandomizeLetterSize(newPassword);
            }

            InsertSymbols(
                symbolMode,
                symbolCount,
                useLetters,
                useBigLetters,
                useNumbers,
                useSymbols,
                ref newPassword
            );

            return newPassword.Trim();
        }

        public double CalculateEntropy(string password)
        {
            int uniqueSymbols = 0;

            if (password.Any(char.IsLower))
            {
                uniqueSymbols += 26;
            }

            if (password.Any(char.IsUpper))
            {
                uniqueSymbols += 26;
            }

            if (password.Any(char.IsDigit))
            {
                uniqueSymbols += 10;
            }

            if (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation))
            {
                uniqueSymbols += Refs.dataBank.SpecialCharsCount;
            }

            double entropy = Math.Log(Math.Pow(uniqueSymbols, password.Length));
            return Math.Round(entropy, 2);
        }
        #endregion Methods (public)

        #region Methods (private)
        private void PrepareCharacterList(
            int wordCount,
            bool useLetters,
            bool useBigLetters,
            bool useNumbers,
            bool useSymbols
        )
        {
            _availableCharacters.Clear();

            if (useLetters)
            {
                for (int i = 0; i < 26; i++)
                {
                    _availableCharacters.Add((char)('a' + i));
                }
            }

            if (useBigLetters)
            {
                for (int i = 0; i < 26; i++)
                {
                    _availableCharacters.Add((char)('A' + i));
                }
            }

            if (useNumbers)
            {
                for (int i = 48; i < 58; i++)
                {
                    _availableCharacters.Add((char)i);
                }
            }

            if (useSymbols)
            {
                _availableCharacters.AddRange(Refs.dataBank.Symbols);
            }

            if (wordCount > 0 && !useSymbols)
            {
                _availableCharacters.Add(' ');
            }
        }

        private void InsertWords(int wordCount, ref string newPassword)
        {
            if (Refs.dataBank.WordList.Count < 1)
            {
                new ErrorHandler("WordListEmpty");
            }

            for (int i = 0; i < wordCount; i++)
            {
                newPassword += Refs.dataBank.WordList[_randomIndex.Next(0, Refs.dataBank.WordList.Count)] + " ";
            }
        }

        private void InsertSymbols(
            int symbolMode,
            int symbolCount,
            bool useLetters,
            bool useBigLetters,
            bool useNumbers,
            bool useSymbols,
            ref string newPassword
        )
        {
            if (useLetters || useBigLetters || useNumbers || useSymbols)
            {
                switch (symbolMode)
                {
                    case (int)SymbolMode.RandomSpacing:
                        newPassword = RandomizeSpacing(newPassword, symbolCount);
                        break;

                    case (int)SymbolMode.ReplaceCharacters:
                        newPassword = ReplaceRandomCharacters(newPassword, symbolCount);
                        break;

                    case (int)SymbolMode.RandomInsert:
                        newPassword = InsertRandomCharacters(newPassword, symbolCount);
                        break;

                    default:
                        break;
                }
            }
        }

        private char GetRandomSymbol()
        {
            if (_availableCharacters.Count < 1)
            {
                return ' ';
            }

            return _availableCharacters[_randomIndex.Next(0, _availableCharacters.Count)];
        }

        private string RandomizeSpacing(string password, int characterCount)
        {
            string newPassword = password.Trim();
            int spacePosition = newPassword.IndexOf(' ');

            while (spacePosition > 0)
            {
                int spacingWidth = _randomIndex.Next(0, characterCount);
                string newSpacing = "";

                for (int i = 0; i < spacingWidth; i++)
                {
                    newSpacing += GetRandomSymbol();
                }

                newPassword = newPassword.Substring(0, spacePosition) + newSpacing + newPassword.Substring(spacePosition + 1);
                spacePosition = newPassword.IndexOf(' ', spacePosition + newSpacing.Length + 1);
            }

            return newPassword;
        }

        private string RandomizeLetterSize(string password)
        {
            char[] newPassword = password.ToCharArray();
            int charactersToChange = _randomIndex.Next(0, newPassword.Length);

            for (int i = 0; i < charactersToChange; i++)
            {
                int characterIndex = _randomIndex.Next(0, newPassword.Length);
                newPassword[characterIndex] = char.ToUpper(newPassword[characterIndex]);
            }

            return new string(newPassword);
        }

        private string ReplaceRandomCharacters(string password, int characterCount)
        {
            char[] newPassword = password.ToCharArray();

            for (int i = 0; i < characterCount; i++)
            {
                newPassword[_randomIndex.Next(0, newPassword.Length)] = GetRandomSymbol();
            }

            return new string(newPassword);
        }

        private string InsertRandomCharacters(string password, int characterCount)
        {
            string newPassword = password;

            for (int i = 0; i < characterCount; i++)
            {
                int charPosition = (newPassword.Length > 0) ? _randomIndex.Next(0, newPassword.Length) : 0;

                if (charPosition > 0)
                {
                    newPassword = newPassword.Substring(0, charPosition) + GetRandomSymbol() + newPassword.Substring(charPosition);
                }
                else
                {
                    newPassword = GetRandomSymbol() + newPassword;
                }
            }

            return newPassword;
        }
        #endregion Methods (private)
    }
}
