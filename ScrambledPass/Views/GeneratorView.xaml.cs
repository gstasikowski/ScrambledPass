using Microsoft.Win32;
using ScrambledPass.Interfaces;
using ScrambledPass.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScrambledPass.Views
{
    /// <summary>
    /// Interaction logic for GeneratorView.xaml
    /// </summary>
    public partial class GeneratorView : IPageViewModel
    {
        App app = (App)App.Current;
        int charMode = -1;

        public GeneratorView()
        {
            InitializeComponent();
            InitialSetup();

            CBRandomCharactersPanel_Click(this, null);
            CBRandomWordsPanel_Click(this, null);
        }

        #region Methods
        private void InitialSetup()
        {
            Refs.fileOperations.LoadTranslations();
            Refs.fileOperations.LoadSettings();

            txtb_wordCount.Text = Refs.dataBank.GetSetting("defWordCount");
            txtb_charCount.Text = Refs.dataBank.GetSetting("defCharCount");

            if (Refs.dataBank.GetSetting("rememberLastWordList") == "True")
                Refs.passGen.PrepareWordList(Refs.dataBank.GetSetting("lastWordList"));
            else
                Refs.passGen.PrepareWordList(string.Empty);

            app.appReady = true;
        }

        private void SetWordsPanelStatus()
        {
            pnl_words.IsEnabled = (bool)chkb_randWords.IsChecked;
            pnl_words.Visibility = (bool)chkb_randWords.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            CheckPasswordRules();
        }

        private void SetCharPanelStatus()
        {
            pnl_chars.IsEnabled = (bool)chkb_randChars.IsChecked;
            pnl_chars.Visibility = (bool)chkb_randChars.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            CheckPasswordRules();
        }

        private void GeneratePassword()
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

        private void SetCharacterMode(string characterMode)
        {
            int.TryParse(characterMode, out charMode);
        }

        private void UpdatePasswordStrength()
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

        private void LoadCustomWordList()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
                Refs.passGen.PrepareWordList(fileDialog.FileName);
        }

        private void LoadDefaultWordList()
        {
            Refs.passGen.PrepareWordList(string.Empty);
        }

        private void ToggleSettingsMenu()
        {
            Refs.viewControl.CurrentPageViewModel = Refs.viewControl.PageViewModels[1]; // switch to binding
        }
        #endregion

        #region UI Events
        private void CBRandomWordsPanel_Click(object sender, RoutedEventArgs e)
        {
            SetWordsPanelStatus();
        }

        private void CBRandomCharactersPanel_Click(object sender, RoutedEventArgs e)
        {
            SetCharPanelStatus();
        }

        private void BtnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            GeneratePassword();
        }
                
        private void RBCharacterMode_Checked(object sender, RoutedEventArgs e)
        {
            SetCharacterMode((sender as System.Windows.Controls.RadioButton).Tag as string);
        }

        private void TAPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePasswordStrength();
        }

        private void BtnLoadCustomWordList_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomWordList();
        }

        private void BtnLoadDefWordList_Click(object sender, RoutedEventArgs e)
        {
            LoadDefaultWordList();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            ToggleSettingsMenu();
        }
        #endregion
    }
}
