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
            cboLanguage.Items.Clear();

            foreach (string language in Refs.dataBank.LanguageList)
            { cboLanguage.Items.Add(language.Substring(0, language.IndexOf('(') - 1)); }

            cboLanguage.SelectedIndex = Refs.dataBank.LanguageIndex(Refs.dataBank.GetSetting("languageID"));
            SelectLanguage();

            // theme
            cboTheme.ItemsSource = Refs.dataBank.ThemeList;
            cboTheme.SelectedIndex = Refs.dataBank.ThemeIndex(Refs.dataBank.GetSetting("theme"));
            SelectTheme();

            // toggles
            chkLoadCustomWordList.IsChecked = Refs.dataBank.GetSetting("rememberLastWordList") == "True";
            chkClearClipboard.IsChecked = Refs.dataBank.GetSetting("clearClipboard") == "True";
            txtClearClipboardDelay.IsEnabled = (bool)chkClearClipboard.IsChecked;

            // textboxes (content)
            txtDefaultWordCount.Text = Refs.dataBank.GetSetting("defaultWordCount");
            txtDefaultCharCount.Text = Refs.dataBank.GetSetting("defaultCharCount");
            txtClearClipboardDelay.Text = Refs.dataBank.GetSetting("clearClipboardDelay");

            app.appReady = true;
        }

        private void SelectLanguage()
        {
            string currentLanguage = Refs.dataBank.LanguageList[cboLanguage.SelectedIndex];
            int codePosition = currentLanguage.IndexOf('[') + 1;
            string cultureCode = currentLanguage.Substring(codePosition, currentLanguage.Length - (codePosition + 1));
            Refs.dataBank.SetSetting("languageID", cultureCode);
            Refs.resourceHandler.SwitchLanguage(cultureCode);
            FileOperations.SaveSettings();
        }

        private void SelectTheme()
        {
            string selectedTheme = Refs.dataBank.ThemeList[cboTheme.SelectedIndex];
            Refs.dataBank.SetSetting("theme", selectedTheme);
            Refs.resourceHandler.SwitchTheme(selectedTheme);
            FileOperations.SaveSettings();
        }
        #endregion Methods

        #region UI Events
        private void SelectLanguage(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectLanguage();
        }
        private void SelectTheme(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectTheme();
        }

        private void ToggleCustomWordListReload(object sender, RoutedEventArgs e)
        {
            Refs.dataBank.SetSetting("rememberLastWordList", chkLoadCustomWordList.IsChecked.ToString());
            FileOperations.SaveSettings();
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
                Refs.dataBank.SetSetting("defaultWordCount", txtDefaultWordCount.Text);
                FileOperations.SaveSettings();
            }
        }

        private void SetDefCharCount(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (app.appReady)
            {
                Refs.dataBank.SetSetting("defaultCharCount", txtDefaultCharCount.Text);
                FileOperations.SaveSettings();
            }
        }

        private void ToggleClipboardClearing(object sender, RoutedEventArgs e)
        {
            if (app.appReady)
            {
                txtClearClipboardDelay.IsEnabled = (bool)chkClearClipboard.IsChecked;
                Refs.dataBank.SetSetting("clearClipboard", chkClearClipboard.IsChecked.ToString());
                FileOperations.SaveSettings();
            }
        }

        private void SetClipboardClearingDelay(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (app.appReady)
            {
                Refs.dataBank.SetSetting("clearClipboardDelay", txtClearClipboardDelay.Text);
                FileOperations.SaveSettings();
            }
        }
        #endregion UI Events
    }
}
