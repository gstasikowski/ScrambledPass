using Microsoft.Win32;
using ScrambledPass.Logic;
using System.Windows;
using System.Windows.Media;

namespace ScrambledPass.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        App app = (App)App.Current;
        int charMode = -1;

        public MainWindow()
        {
            InitializeComponent();
            InitialSetup();

            SetCharPanelStatus(this, null);
            SetWordsPanelStatus(this, null);
        }

        private void InitialSetup()
        {
            DataContext = Refs.dataBank;
            Refs.fileOperations.LoadTranslations();
            Refs.fileOperations.LoadSettings();
            RefreshUI();

            if (Refs.dataBank.GetSetting("rememberLastWordList") == "True")
                Refs.passGen.PrepareWordList(Refs.dataBank.GetSetting("lastWordList"));
            else
                Refs.passGen.PrepareWordList(string.Empty);

            pnl_layout.Visibility = Visibility.Visible;
            pnl_settings.Visibility = Visibility.Collapsed;

            app.appReady = true;
        }

        private void SetWordsPanelStatus(object sender, RoutedEventArgs e)
        {
            pnl_words.IsEnabled = (bool)chkb_randWords.IsChecked;
            pnl_words.Visibility = (bool)chkb_randWords.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            CheckPasswordRules();
        }

        private void SetCharPanelStatus(object sender, RoutedEventArgs e)
        {
            pnl_chars.IsEnabled = (bool)chkb_randChars.IsChecked;
            pnl_chars.Visibility = (bool)chkb_randChars.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            CheckPasswordRules();
        }

        private void GeneratePassword(object sender, RoutedEventArgs e)
        {
            if (pnl_chars.IsEnabled)
                txtb_passTarget.Text = Refs.passGen.GeneratePassword(CheckWordProps(), charMode, CheckCharsProps(), (bool)chkb_randLetterSize.IsChecked, (bool)chkb_randLower.IsChecked, (bool)chkb_randUpper.IsChecked, (bool)chkb_randNumbers.IsChecked, (bool)chkb_randSpcChar.IsChecked);
            else
                txtb_passTarget.Text = Refs.passGen.GeneratePassword(CheckWordProps(), -1, 0, (bool)chkb_randLetterSize.IsChecked, false, false, false, false);
        }

        private void CheckPasswordRules()
        {
            btn_generate.IsEnabled = (bool)chkb_randWords.IsChecked || (bool)chkb_randChars.IsChecked;
            rb_replaceRandChar.IsEnabled = rb_replaceWhiteChar.IsEnabled = (bool)chkb_randWords.IsChecked;

            if (!(bool)chkb_randWords.IsChecked)
            {
                rb_randPos.IsChecked = true;
            }
        }

        private int CheckWordProps()
        {
            if (!pnl_words.IsEnabled)
                return 0;

            int wordCount = 0;

            if (!int.TryParse(txtb_wordCount.Text, out wordCount))
            { new ErrorHandler("parserError"); }

            return wordCount;
        }

        private int CheckCharsProps()
        {
            if (!pnl_chars.IsEnabled)
                return 0;

            int charCount = 0;

            if (!int.TryParse(txtb_charCount.Text, out charCount))
            { new ErrorHandler("parserError"); }

            return charCount;
        }

        private void SetCharacterMode(object sender, RoutedEventArgs e)
        {
            int.TryParse((sender as System.Windows.Controls.RadioButton).Tag as string, out charMode);
        }

        private void UpdatePasswordStrength(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            double passEntropy = Refs.passGen.CalculateEntropy(txtb_passTarget.Text);
            string strengthTxt = "";
            lbl_passStrength.Foreground = Brushes.White;

            if (passEntropy > 0.0)
            {
                strengthTxt = (string)FindResource("UIPassWeak");
                lbl_passStrength.Foreground = Brushes.Red;
            }

            if (passEntropy >= 65.0)
            {
                strengthTxt = (string)FindResource("UIPassGood");
                lbl_passStrength.Foreground = Brushes.Blue;
            }

            if (passEntropy >= 100.0)
            {
                strengthTxt = (string)FindResource("UIPassGreat");
                lbl_passStrength.Foreground = Brushes.Green;
            }

            lbl_passEntropy.Content = string.Format("{0}: {1} bit", (string)FindResource("UIEntropy"), passEntropy);
            lbl_passStrength.Content = string.Format("{0}: {1}", (string)FindResource("UIPassStrength"), strengthTxt);
        }

        private void LoadCustomWordList(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
                Refs.passGen.PrepareWordList(fileDialog.FileName);
        }

        private void LoadDefaultWordList(object sender, RoutedEventArgs e)
        {
            Refs.passGen.PrepareWordList(string.Empty);
        }

        private void SelectLanguage(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string currentLanguage = cb_language.SelectedItem.ToString();
            int codePosition = currentLanguage.IndexOf('[') + 1;
            string cultureCode = currentLanguage.Substring(codePosition, currentLanguage.Length - (codePosition + 1));
            Refs.dataBank.SetSetting("languageID", cultureCode);
            Refs.fileOperations.SaveSettings();
            Refs.localizationHandler.SwitchLanguage(cultureCode);
        }

        private void ToggleCustomWordListReload(object sender, RoutedEventArgs e)
        {
            Refs.dataBank.SetSetting("rememberLastWordList", chkb_loadCustomWordList.IsChecked.ToString());
            Refs.fileOperations.SaveSettings();
        }

        private void ToggleSettingsMenu(object sender, RoutedEventArgs e)
        {
            pnl_layout.Visibility = (pnl_layout.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            pnl_settings.Visibility = (pnl_settings.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void RestoreDefaultSettings(object sender, RoutedEventArgs e)
        {
            Refs.dataBank.DefaultSettings();
            RefreshUI();
        }

        private void RefreshUI()
        {
            int languageID = Refs.dataBank.LanguageIndex(Refs.dataBank.GetSetting("languageID"));
            cb_language.SelectedIndex = languageID;


            if (Refs.dataBank.GetSetting("rememberLastWordList") == "True")
                chkb_loadCustomWordList.IsChecked = true;
            else
                chkb_loadCustomWordList.IsChecked = false;

            txtb_wordCount.Text = txtb_defWordCount.Text = Refs.dataBank.GetSetting("defWordCount");
            txtb_charCount.Text = txtb_defCharCount.Text = Refs.dataBank.GetSetting("defCharCount");
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
    }
}
