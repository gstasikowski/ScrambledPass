using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrambledPass.Logic
{
    public class Generator
    {
        Random randIndex = new Random();
        List<char> availableCharList = new List<char>();

        public Generator()
        {

        }

        #region Methods (public)
        public string GeneratePassword(int wordCount, int charMode, int charCount, bool randCharSize, bool letters, bool bigLetters, bool numbers, bool specialChars)
        {
            string newPassword = "";
            PrepareCharList(wordCount, letters, bigLetters, numbers, specialChars);

            for (int i = 0; i < wordCount; i++)
                newPassword += Refs.dataBank.WordList[randIndex.Next(0, Refs.dataBank.WordList.Count)] + " ";

            if (randCharSize)
                newPassword = RandomizeLetterSize(newPassword);

            switch (charMode)
            {
                case 0:
                    newPassword = RandomizeSpacing(newPassword, charCount);
                    break;

                case 1:
                    newPassword = ReplaceRandomCharacters(newPassword, charCount);
                    break;

                case 2:
                    newPassword = InsertRandomCharacters(newPassword, charCount);
                    break;

                default:
                    break;
            }

            return newPassword.Trim();
        }

        public void PrepareWordList(string filePath)
        {
            Refs.dataBank.WordList.Clear();

            if (filePath == string.Empty)
                Refs.dataBank.WordList.AddRange(Refs.fileOperations.LoadDefaultWordList());
            else
                Refs.dataBank.WordList.AddRange(Refs.fileOperations.LoadCustomWordList(filePath));
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
        void PrepareCharList(int wordCount, bool letters, bool bigLetters, bool numbers, bool specialChars)
        {
            availableCharList.Clear();

            if (letters)
            {
                for (int i = 0; i < 26; i++)
                    availableCharList.Add((char)('a' + i));
            }

            if (bigLetters)
            {
                for (int i = 0; i < 26; i++)
                    availableCharList.Add((char)('A' + i));
            }

            if (numbers)
            {
                for (int i = 48; i < 58; i++)
                    availableCharList.Add((char)i);
            }

            if (specialChars)
                availableCharList.AddRange(Refs.dataBank.SpecialChars);

            if (wordCount > 0 && !specialChars)
                availableCharList.Add(' ');
        }


        char GetRandomCharacter()
        {
            if (availableCharList.Count < 1)
                return ' ';

            return availableCharList[randIndex.Next(0, availableCharList.Count)];
        }

        string RandomizeSpacing(string password, int charCount)
        {
            string newPassword = password.Trim();
            int spacePos = newPassword.IndexOf(' ');

            while (spacePos > 0)
            {
                int spacingWidth = randIndex.Next(0, charCount);
                string newSpacing = "";

                for (int i = 0; i < spacingWidth; i++)
                    newSpacing += GetRandomCharacter();

                newPassword = newPassword.Substring(0, spacePos) + newSpacing + newPassword.Substring(spacePos + 1);
                spacePos = newPassword.IndexOf(' ', spacePos + newSpacing.Length + 1);
            }

            return newPassword;
        }

        string RandomizeLetterSize(string password)
        {
            char[] newPassword = password.ToCharArray();
            int charChanges = randIndex.Next(0, newPassword.Length);

            for (int i = 0; i < charChanges; i++)
            {
                int charPosition = randIndex.Next(0, newPassword.Length);
                newPassword[charPosition] = char.ToUpper(newPassword[charPosition]);
            }

            return new string(newPassword);
        }

        string ReplaceRandomCharacters(string password, int charCount)
        {
            char[] newPassword = password.ToCharArray();

            for (int i = 0; i < charCount; i++)
                newPassword[randIndex.Next(0, newPassword.Length)] = GetRandomCharacter();

            return new string(newPassword);
        }

        string InsertRandomCharacters(string password, int charCount)
        {
            string newPassword = password;

            for (int i = 0; i < charCount; i++)
            {
                int charPosition = (newPassword.Length > 0) ? randIndex.Next(0, newPassword.Length) : 0;

                if (charPosition > 0)
                    newPassword = newPassword.Substring(0, charPosition) + GetRandomCharacter() + newPassword.Substring(charPosition);
                else
                    newPassword = GetRandomCharacter() + newPassword;
            }

            return newPassword;
        }
        #endregion Methods (private)
    }
}
