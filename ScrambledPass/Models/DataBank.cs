using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ScrambledPass.Models
{
    public class DataBank
    {
        #region Variables
        static string defaultWordListFile = "ScrambledPass.Resources.defaultWordList.txt";
        readonly string defaultConfigFile = AppDomain.CurrentDomain.BaseDirectory + "Config.xml";
        readonly string defaultLanguagePath = AppDomain.CurrentDomain.BaseDirectory + "Languages\\";
        readonly string defaultThemePath = AppDomain.CurrentDomain.BaseDirectory + "Themes\\";
        
        Dictionary<string, string> settings = new Dictionary<string, string>();
        List<string> wordList = new List<string>();
        static readonly char[] specialChars = { ' ', '.', ',', ';', '/', '\\', '\'', '[', ']', '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '|', ':', '\"', '<', '>', '?' };

        private ObservableCollection<string> languageList = new ObservableCollection<string>();
        private ObservableCollection<string> themeList = new ObservableCollection<string>();
        #endregion Variables

        #region Properties
        public string DefaultWordListFile
        {
            get { return defaultWordListFile; }
        }

        public string DefaultConfigFile
        {
            get { return defaultConfigFile; }
        }

        public string DefaultLanguagePath
        {
            get { return defaultLanguagePath; }
        }

        public string DefaultThemePath
        {
            get { return defaultThemePath; }
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

        public ObservableCollection<string> LanguageList
        {
            get { return languageList; }
            set { languageList = value; }
        }
        public ObservableCollection<string> ThemeList
        {
            get { return themeList; }
            set { themeList = value; }
        }
        #endregion Properties

        #region Methods
        public void DefaultSettings()
        {
            SetSetting("languageID", "en-US");
            SetSetting("theme", "Light");
            SetSetting("lastWordList", string.Empty);
            SetSetting("rememberLastWordList", "False");
            SetSetting("defaultWordCount", "5");
            SetSetting("defaultCharCount", "5");
            SetSetting("clearClipboard", "False");
            SetSetting("clearClipboardDelay", "15");
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

        
        public void AddAvailableLanguage(string languageCode)
        {
            if (!languageList.Contains(languageCode))
            { languageList.Add(languageCode); }
        }

        public int LanguageIndex(string languageCode)
        {
            int languageIndex = languageList.IndexOf(languageCode);
            return (languageIndex > -1) ? languageIndex : 0;
        }

        public void AddAvailableTheme(string themeName)
        {
            if (!themeList.Contains(themeName))
            { themeList.Add(themeName); }
        }

        public int ThemeIndex(string themeName)
        {
            int themeIndex = themeList.IndexOf(themeName);
            return (themeIndex > -1) ? themeIndex : 0;
        }
        #endregion Methods
    }
}
