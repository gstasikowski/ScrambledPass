using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ScrambledPass.Model
{
    public class DataBank
    {
        List<string> wordList = new List<string>();
        char[] specialChars = { ' ', '.', ',', ';', '/', '\\', '\'', '[', ']', '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '|', ':', '\"', '<', '>', '?' };

        Dictionary<string, List<string>> UIText = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> ToolTips = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> ErrorMessages = new Dictionary<string, List<string>>();

        ObservableCollection<string> cmbContent = new ObservableCollection<string>();

        public void DefaultSettings()
        {
            Properties.Settings.Default["languageID"] = (cmbContent.IndexOf("EN") >= 0 ? cmbContent.IndexOf("EN") : 0).ToString();
            Properties.Settings.Default["lastWordList"] = string.Empty;
            Properties.Settings.Default["rememberLastWordList"] = "False";
            Properties.Settings.Default["defWordCount"] = "5";
            Properties.Settings.Default["defCharCount"] = "5";
            Properties.Settings.Default.Save();
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

        public string GetErrorMessage(string errorCode, int lang)
        {
            string message;

            try
            { message = ErrorMessages[errorCode][lang]; }
            catch
            { message = ErrorMessages["default"][0]; }

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

        public ObservableCollection<string> CmbContent
        { 
            get { return cmbContent; }
            set { cmbContent = value; } 
        }

        public void AddAvailableLanguage(string languageCode)
        {
            if (!cmbContent.Contains(languageCode))
            { cmbContent.Add(languageCode); }
        }

        public void FillLanguageDictionary(string target, Dictionary<string, string> translation)
        {
            // merge with ApplyTranslation()
            switch (target)
            {
                case "UI":
                    ApplyTranslation(UIText, translation);
                    break;

                case "ToolTips":
                    ApplyTranslation(ToolTips, translation);
                    break;

                case "ErrorMessages":
                    ApplyTranslation(ErrorMessages, translation);
                    break;

                default:
                    return;
            }
        }

        void ApplyTranslation(Dictionary<string, List<string>> target, Dictionary<string, string> translation)
        {
            foreach (var element in translation)
            {
                if (target.ContainsKey(element.Key))
                {
                    target[element.Key].Add(element.Value);
                }
                else
                {
                    target.Add(element.Key, new List<string>() { element.Value });
                }
            }
        }
    }
}
