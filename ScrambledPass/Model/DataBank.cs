using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ScrambledPass.Model
{
    public class DataBank
    {
        string defaultConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        string defaultLanguagePath = AppDomain.CurrentDomain.BaseDirectory + "Languages\\";
        string defaultWordList = "ScrambledPass.Resources.defaultWordList.txt";
        Dictionary<string, string> settings = new Dictionary<string, string>();

        List<string> wordList = new List<string>();
        char[] specialChars = { ' ', '.', ',', ';', '/', '\\', '\'', '[', ']', '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '|', ':', '\"', '<', '>', '?' };
        
        ObservableCollection<string> langList = new ObservableCollection<string>();
        List<string> languageList = new List<string>();

        public string DefaultConfigPath
        {
            get { return defaultConfigPath; }
        }

        public string DefaultLanguagePath
        {
            get { return defaultLanguagePath; }
        }
        public string DefaultWordList
        {
            get { return defaultWordList; }
        }

        public void DefaultSettings()
        {
            SetSetting("languageID", "en-US");
            SetSetting("lastWordList", string.Empty);
            SetSetting("rememberLastWordList", "False");
            SetSetting("defWordCount", "5");
            SetSetting("defCharCount", "5");
        }

        public Dictionary<string, string> GetAllSettings()
        {
            return settings;
        }

        public string GetSetting(string key)
        {
            return settings[key];
        }

        public void SetSetting(string key, string value)
        {
            if (settings.ContainsKey(key))
            { settings[key] = value; }
            else
            { settings.Add(key, value); }
        }

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

        public ObservableCollection<string> CmbContent
        { 
            get { return langList; }
            set { langList = value; } 
        }
        
        public void AddAvailableLanguage(string languageCode)
        {
            if (!langList.Contains(languageCode))
            { langList.Add(languageCode); }
        }

        public List<string> LanguageList
        {
            get { return languageList; }
        }

        public int LanguageIndex(string languageCode)
        {
            for (int i = 0; i < langList.Count; i++)
            {
                if (langList[i].Contains(languageCode))
                    return i;
            }

            return 0;
        }
    }
}
