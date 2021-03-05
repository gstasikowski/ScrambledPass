using System.Collections.Generic;

namespace ScrambledPass.Model
{
    public class DataBank
    {
        List<string> wordList = new List<string>();
        char[] specialCharacters = { ',', '.', '/', ';', '\'', '[', ']', '\\', '~', '!', '@', '$', '%', '^', '&', '*', '(', ')', '-', '+' };

        Dictionary<string, string> errorMessages = new Dictionary<string, string> {
            { "default", "Unkown error has occured. Please try again." },
            { "parserError", "Word and character count fields require numerical input.\nPlease fix problematic values and try again." },
            { "fileNotFound", "Word list file can't be accessed." }
        };

        public List<string> WordList
        {
            get { return wordList; }
        }

        public char[] SpecialCharacters
        { 
            get { return specialCharacters; }
        }

        public string GetErrorMessage(string errorCode)
        {
            string message;

            try
            { message = errorMessages[errorCode]; }
            catch
            { message = errorMessages["default"]; }

            return message;
        }
    }
}
