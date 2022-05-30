using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrambledPass.Logic
{
    public class Generator
    {
        Random randomIndex = new Random();
        List<char> availableCharList = new List<char>();

        public Generator()
        {

        }

        #region Methods (public)
        public string GeneratePassword(int wordCount, int symbolMode, int symbolCount, bool randomCharSize, bool useLetters, bool useBigLetters, bool useNumbers, bool useSymbols)
        {
            string newPassword = "";
            PrepareCharList(wordCount, useLetters, useBigLetters, useNumbers, useSymbols);

            for (int i = 0; i < wordCount; i++)
                newPassword += Refs.dataBank.WordList[randomIndex.Next(0, Refs.dataBank.WordList.Count)] + " ";

            if (randomCharSize)
                newPassword = RandomizeLetterSize(newPassword);

            if (useLetters || useBigLetters || useNumbers || useSymbols)
            {
                switch (symbolMode)
                {
                    case 0:
                        newPassword = RandomizeSpacing(newPassword, symbolCount);
                        break;

                    case 1:
                        newPassword = ReplaceRandomCharacters(newPassword, symbolCount);
                        break;

                    case 2:
                        newPassword = InsertRandomCharacters(newPassword, symbolCount);
                        break;

                    default:
                        break;
                }
            }

            return newPassword.Trim();
        }

        public void PrepareWordList(string filePath)
        {
            Refs.dataBank.WordList.Clear();

            if (filePath == string.Empty)
                Refs.dataBank.WordList.AddRange(FileOperations.LoadDefaultWordList());
            else
                Refs.dataBank.WordList.AddRange(FileOperations.LoadCustomWordList(filePath));
        }

        public double CalculateEntropy(string password)
        {
            int uniqueSymbols = 0;

            if (password.Any(char.IsLower))
                uniqueSymbols += 26;

            if (password.Any(char.IsUpper))
                uniqueSymbols += 26;

            if (password.Any(char.IsDigit))
                uniqueSymbols += 10;

            if (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation))
                uniqueSymbols += Refs.dataBank.SpecialCharsCount;

            double entropy = Math.Log(Math.Pow(uniqueSymbols, password.Length));
            return Math.Round(entropy, 2);
        }
        #endregion Methods (public)

        #region Methods (private)
        void PrepareCharList(int wordCount, bool useLetters, bool useBigLetters, bool useNumbers, bool useSymbols)
        {
            availableCharList.Clear();

            if (useLetters)
            {
                for (int i = 0; i < 26; i++)
                    availableCharList.Add((char)('a' + i));
            }

            if (useBigLetters)
            {
                for (int i = 0; i < 26; i++)
                    availableCharList.Add((char)('A' + i));
            }

            if (useNumbers)
            {
                for (int i = 48; i < 58; i++)
                    availableCharList.Add((char)i);
            }

            if (useSymbols)
                availableCharList.AddRange(Refs.dataBank.Symbols);

            if (wordCount > 0 && !useSymbols)
                availableCharList.Add(' ');
        }


        char GetRandomSymbol()
        {
            if (availableCharList.Count < 1)
                return ' ';

            return availableCharList[randomIndex.Next(0, availableCharList.Count)];
        }

        string RandomizeSpacing(string password, int charCount)
        {
            string newPassword = password.Trim();
            int spacePosition = newPassword.IndexOf(' ');

            while (spacePosition > 0)
            {
                int spacingWidth = randomIndex.Next(0, charCount);
                string newSpacing = "";

                for (int i = 0; i < spacingWidth; i++)
                    newSpacing += GetRandomSymbol();

                newPassword = newPassword.Substring(0, spacePosition) + newSpacing + newPassword.Substring(spacePosition + 1);
                spacePosition = newPassword.IndexOf(' ', spacePosition + newSpacing.Length + 1);
            }

            return newPassword;
        }

        string RandomizeLetterSize(string password)
        {
            char[] newPassword = password.ToCharArray();
            int charChanges = randomIndex.Next(0, newPassword.Length);

            for (int i = 0; i < charChanges; i++)
            {
                int charPosition = randomIndex.Next(0, newPassword.Length);
                newPassword[charPosition] = char.ToUpper(newPassword[charPosition]);
            }

            return new string(newPassword);
        }

        string ReplaceRandomCharacters(string password, int charCount)
        {
            char[] newPassword = password.ToCharArray();

            for (int i = 0; i < charCount; i++)
                newPassword[randomIndex.Next(0, newPassword.Length)] = GetRandomSymbol();

            return new string(newPassword);
        }

        string InsertRandomCharacters(string password, int charCount)
        {
            string newPassword = password;

            for (int i = 0; i < charCount; i++)
            {
                int charPosition = (newPassword.Length > 0) ? randomIndex.Next(0, newPassword.Length) : 0;

                if (charPosition > 0)
                    newPassword = newPassword.Substring(0, charPosition) + GetRandomSymbol() + newPassword.Substring(charPosition);
                else
                    newPassword = GetRandomSymbol() + newPassword;
            }

            return newPassword;
        }
        #endregion Methods (private)
    }
}
