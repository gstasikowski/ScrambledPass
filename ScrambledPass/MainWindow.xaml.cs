using Microsoft.Win32;
using ScrambledPass.Logic;
using System.Windows;

// TODO 
//      1. multi-language UI
//      2. proper password entropy calculations
//      3. optional clipboard cleaning after copying/cutting from password textbox?

namespace ScrambledPass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Generator passGen = new Generator();
        int charMode = -1;

        public MainWindow()
        {
            InitializeComponent();

            SetCharPanelStatus(this, null);
            SetWordsPanelStatus(this, null);
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
                txtb_passTarget.Text = passGen.GeneratePassword(CheckWordProps(), charMode, CheckCharsProps(), (bool)chkb_randCharSize.IsChecked, (bool)chkb_randLetters.IsChecked, (bool)chkb_randBigLetters.IsChecked, (bool)chkb_randNumbers.IsChecked, (bool)chkb_randSpcChar.IsChecked);
            else
                txtb_passTarget.Text = passGen.GeneratePassword(CheckWordProps(), -1, 0, (bool)chkb_randCharSize.IsChecked, false, false, false, false);
        }

        private void CheckPasswordRules()
        {
            btn_generate.IsEnabled = ((bool)chkb_randWords.IsChecked || (bool)chkb_randChars.IsChecked);
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
            // TODO change to properly calculated ENTROPY
            lbl_info.Content = "Password strength: " + (txtb_passTarget.Text.Length * 8).ToString() + " bit";
        }

        private void LoadCustomWordList(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
                passGen.PrepareWordList(fileDialog.FileName);
        }

        private void LoadDefaultWordList(object sender, RoutedEventArgs e)
        {
            passGen.PrepareWordList(string.Empty);
        }
    }
}
