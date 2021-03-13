using System.Collections.Generic;

namespace ScrambledPass.Model
{
    public class DataBank
    {
        List<string> wordList = new List<string>();
        char[] specialChars = { ' ', '.', ',', ';', '/', '\\', '\'', '[', ']', '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '|', ':', '\"', '<', '>', '?' };

        Dictionary<string, List<string>> UIText = new Dictionary<string, List<string>> {
            { "words", new List<string>{ "Words", "Wyrazy" } },
            { "wordCount", new List<string>{ "Word count", "Ilość słów" } },
            { "randLetterSize", new List<string>{"Randomize letter size", "Losowy rozmiar liter" } },
            { "characters", new List<string>{"Characters", "Znaki" } },
            { "charCount", new List<string>{"Character count", "Ilość znaków" } },
            { "symbols", new List<string>{"Symbols", "Znaki specjalne" } },
            { "replaceSpacing", new List<string>{"Replace word spacing", "Zastąp odstępy" } },
            { "replaceChars", new List<string>{"Replace random characters", "Zastęp losowe znaki" } },
            { "randomPosition", new List<string>{"Add in random position", "Dodaj w losowej pozycji" } },
            { "password", new List<string>{"Password", "Hasło" } },
            { "entropy", new List<string>{"Entropy", "Entropia" } },
            { "passStrength", new List<string>{"Password strength", "Siła hasła" } },
            { "passWeak", new List<string>{"weak", "słabe" } },
            { "passGood", new List<string>{"good", "dobre" } },
            { "passGreat", new List<string>{"great", "świetne" } },
            { "generate", new List<string>{"Generate", "Generuj" } }
        };

        Dictionary<string, List<string>> ToolTips = new Dictionary<string, List<string>> {
            { "t_words", new List<string>{ "Generate new password using a word list", "Generuj nowe hasło korzystając z listy wyrazów" } },
            { "t_characters", new List<string>{ "Generate new password using random characters", "Generuj nowe hasło używając losowych znaków" } },
            { "t_passGen", new List<string>{ "Generate new password", "Generuj nowe hasło" } },
            { "t_wordList", new List<string>{ "Load custom word list", "Wczytaj własną listę wyrazów" } },
            { "t_defWordList", new List<string>{ "Load default word list", "Wczytaj domyślną listę wyrazów" } },

        };

        Dictionary<string, List<string>> errorMessages = new Dictionary<string, List<string>> {
            { "default", new List<string>{"Unkown error has occured. Please try again.", "Nieznany błąd. Spróbuj ponownie." } },
            { "parserError", new List<string>{"Word and character count fields require numerical input.\nPlease fix problematic values and try again.", "Pola ilości słów i znaków muszą mieć wartość liczbową.\nPopraw błędne wartości i spróbuj ponownie." } },
            { "fileNotFound", new List<string>{"Word list file can't be accessed.", "Plik z listą wyrazów nie jest dostępny."} }
        };

        public List<string> WordList
        {
            get { return wordList; }
        }

        public char[] SpecialChars
        {
            get { return specialChars; }
        }

        public int SpecialCharsCount
        {
            get { return specialChars.Length; }
        }

        public string GetErrorMessage(string errorCode, int lang)
        {
            string message;

            try
            { message = errorMessages[errorCode][lang]; }
            catch
            { message = errorMessages["default"][0]; }

            return message;
        }

        public string GetLocalText(string key, int lang)
        {
            string localizedText;

            try
            { localizedText = UIText[key][lang]; }
            catch
            { localizedText = "<missing_value>"; }

            return localizedText;
        }

        public string GetToolTip(string key, int lang)
        {
            string localizedText;

            try
            { localizedText = ToolTips[key][lang]; }
            catch
            { localizedText = "<missing_value>"; }

            return localizedText;
        }
    }
}
