using System;
using System.Collections.Generic;

namespace ScrambledPass.Logic
{
    public class Generator
    {
        FileOperations fileO = new FileOperations();
        Random randIndex = new Random();

        List<string> wordList = new List<string>();
        char[] specialCharacters = { ',', '.', '/', ';', '\'', '[', ']', '\\', '~', '!', '@', '$', '%', '^', '&', '*', '(', ')', '-', '+' };
        List<char> availableCharList = new List<char>();

        public Generator()
        {
            PrepareWordList(string.Empty);
        }

        public string GeneratePassword(int wordCount, int charMode, int charCount, bool randCharSize, bool letters, bool bigLetters, bool numbers, bool specialChars)
        {
            string newPassword = "";
            PrepareCharList(wordCount, letters, bigLetters, numbers, specialChars);

            for (int i = 0; i < wordCount; i++)
                newPassword += wordList[randIndex.Next(0, wordList.Count)] + " ";

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
            wordList.Clear();

            if (filePath == string.Empty)
                wordList.AddRange(fileO.LoadDefaultWordList());
            else
                wordList.AddRange(fileO.LoadCustomWordList(filePath));
        }

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
                availableCharList.AddRange(specialCharacters);

            if (wordCount > 0)
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
    }
}
