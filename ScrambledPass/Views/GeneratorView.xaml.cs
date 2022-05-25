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
            txtWordCount.Text = Refs.dataBank.GetSetting("defaultWordCount");
            txtCharCount.Text = Refs.dataBank.GetSetting("defaultCharCount");

            if (Refs.dataBank.GetSetting("rememberLastWordList") == "True")
                Refs.generator.PrepareWordList(Refs.dataBank.GetSetting("lastWordList"));
            else
                Refs.generator.PrepareWordList(string.Empty);

            DataObject.AddCopyingHandler(txtPassword, ClipboardProtection);
        }

        private void SetWordsPanelStatus()
        {
            pnlWords.IsEnabled = (bool)chkRandomWords.IsChecked;
            pnlWords.Visibility = (bool)chkRandomWords.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            CheckPasswordRules();
        }

        private void SetCharPanelStatus()
        {
            pnlChars.IsEnabled = (bool)chkRandomChars.IsChecked;
            pnlChars.Visibility = (bool)chkRandomChars.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            CheckPasswordRules();
        }

        private void GeneratePassword()
        {
            if (pnlChars.IsEnabled)
                txtPassword.Text = Refs.generator.GeneratePassword(CheckWordCount(), charMode, CheckCharCount(), (bool)chkRandomLetterSize.IsChecked, (bool)chkRandomLower.IsChecked, (bool)chkRandomUpper.IsChecked, (bool)chkRandomNumbers.IsChecked, (bool)chkRandomSpecialChar.IsChecked);
            else
                txtPassword.Text = Refs.generator.GeneratePassword(CheckWordCount(), -1, 0, (bool)chkRandomLetterSize.IsChecked, false, false, false, false);
        }

        private void CheckPasswordRules()
        {
            btnGenerate.IsEnabled = (bool)chkRandomWords.IsChecked || 
                ((bool)chkRandomChars.IsChecked && ((bool)chkRandomLower.IsChecked || (bool)chkRandomUpper.IsChecked || (bool)chkRandomNumbers.IsChecked || (bool)chkRandomSpecialChar.IsChecked));
            rbReplaceRandomChar.IsEnabled = rbReplaceWhiteChar.IsEnabled = (bool)chkRandomWords.IsChecked;

            if (!(bool)chkRandomWords.IsChecked)
            {
                rbRandomPos.IsChecked = true;
            }
        }

        private int CheckWordCount()
        {
            if (!pnlWords.IsEnabled)
                return 0;

            int wordCount;

            if (!int.TryParse(txtWordCount.Text, out wordCount))
            { new ErrorHandler("ErrorParser"); }

            return wordCount;
        }

        private int CheckCharCount()
        {
            if (!pnlChars.IsEnabled)
                return 0;

            int charCount;

            if (!int.TryParse(txtCharCount.Text, out charCount))
            { new ErrorHandler("ErrorParser"); }

            return charCount;
        }

        private void SetCharacterMode(string characterMode)
        {
            int.TryParse(characterMode, out charMode);
        }

        private void UpdatePasswordStrength()
        {
            double passEntropy = Refs.generator.CalculateEntropy(txtPassword.Text);
            string strengthTxt = "";
            lblPasswordStrength.Foreground = Brushes.White;

            if (passEntropy > 0.0)
            {
                strengthTxt = (string)FindResource("UIPasswordWeak");
                lblPasswordStrength.Foreground = (Brush)FindResource("PasswordWeak");
            }

            if (passEntropy >= 65.0)
            {
                strengthTxt = (string)FindResource("UIPasswordGood");
                lblPasswordStrength.Foreground = (Brush)FindResource("PasswordGood");
            }

            if (passEntropy >= 100.0)
            {
                strengthTxt = (string)FindResource("UIPasswordGreat");
                lblPasswordStrength.Foreground = (Brush)FindResource("PasswordGreat");
            }

            lblPasswordEntropy.Content = string.Format("{0}: {1} bit", (string)FindResource("UIEntropy"), passEntropy);
            lblPasswordStrength.Content = string.Format("{0}: {1}", (string)FindResource("UIPasswordStrength"), strengthTxt);
        }

        private void LoadCustomWordList()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
                Refs.generator.PrepareWordList(fileDialog.FileName);
        }

        private void LoadDefaultWordList()
        {
            Refs.generator.PrepareWordList(string.Empty);
        }

        private void ToggleSettingsMenu()
        {
            Refs.viewControl.CurrentPageViewModel = Refs.viewControl.PageViewModels[1]; // switch to binding
        }

        private void ClipboardProtection(object sender, DataObjectEventArgs e)
        {
            _ = new Helpers.GeneralHelpers().ClearClipboardAsync();
        }
        #endregion Methods

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
        #endregion  UI Events
    }
}
