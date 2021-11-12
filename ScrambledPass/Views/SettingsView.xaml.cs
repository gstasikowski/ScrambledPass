using ScrambledPass.Interfaces;
using ScrambledPass.Logic;
using System.Windows;

namespace ScrambledPass.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : IPageViewModel
    {
        App app = (App)App.Current;

        public SettingsView()
        {
            InitializeComponent();
            LoadSettings();
        }

        #region Methods
        private void LoadSettings()
        {
            // language
            cb_language.Items.Clear();

            foreach (string language in Refs.dataBank.LanguageList)
            { cb_language.Items.Add(language.Substring(0, language.IndexOf('[') - 1)); }
            
            cb_language.SelectedIndex = Refs.dataBank.LanguageIndex(Refs.dataBank.GetSetting("languageID"));
            SelectLanguage();

            // word list
            if (Refs.dataBank.GetSetting("rememberLastWordList") == "True")
                chkb_loadCustomWordList.IsChecked = true;
            else
                chkb_loadCustomWordList.IsChecked = false;

            // word/char counts
            txtb_defWordCount.Text = Refs.dataBank.GetSetting("defWordCount");
            txtb_defCharCount.Text = Refs.dataBank.GetSetting("defCharCount");
        }

        private void SelectLanguage()
        {
            string currentLanguage = Refs.dataBank.LanguageList[cb_language.SelectedIndex];
            int codePosition = currentLanguage.IndexOf('[') + 1;
            string cultureCode = currentLanguage.Substring(codePosition, currentLanguage.Length - (codePosition + 1));
            Refs.dataBank.SetSetting("languageID", cultureCode);
            Refs.localizationHandler.SwitchLanguage(cultureCode);
            Refs.fileOperations.SaveSettings();
        }
        #endregion

        #region UI Events
        private void SelectLanguage(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectLanguage();
        }

        private void ToggleCustomWordListReload(object sender, RoutedEventArgs e)
        {
            Refs.dataBank.SetSetting("rememberLastWordList", chkb_loadCustomWordList.IsChecked.ToString());
            Refs.fileOperations.SaveSettings();
        }

        private void CloseSettingsView(object sender, RoutedEventArgs e)
        {
            Refs.viewControl.CurrentPageViewModel = Refs.viewControl.PageViewModels[0]; // switch to binding
        }

        private void RestoreDefaultSettings(object sender, RoutedEventArgs e)
        {
            Refs.dataBank.DefaultSettings();
            LoadSettings();
        }

        private void SetDefWordCount(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (app.appReady)
            {
                Refs.dataBank.SetSetting("defWordCount", txtb_defWordCount.Text);
                Refs.fileOperations.SaveSettings();
            }
        }

        private void SetDefCharCount(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (app.appReady)
            {
                Refs.dataBank.SetSetting("defCharCount", txtb_defCharCount.Text);
                Refs.fileOperations.SaveSettings();
            }
        }
        #endregion
    }
}
