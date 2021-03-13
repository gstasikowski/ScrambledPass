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
        Generator passGen = new Generator();
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
            int langID = 0;
            int.TryParse((string)Properties.Settings.Default["languageID"], out langID);
            cb_language.SelectedIndex = langID;
            ApplyLocalizedUI();

            passGen.PrepareWordList((string)Properties.Settings.Default["lastWordList"]);
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
                txtb_passTarget.Text = passGen.GeneratePassword(CheckWordProps(), charMode, CheckCharsProps(), (bool)chkb_randLetterSize.IsChecked, (bool)chkb_randLower.IsChecked, (bool)chkb_randUpper.IsChecked, (bool)chkb_randNumbers.IsChecked, (bool)chkb_randSpcChar.IsChecked);
            else
                txtb_passTarget.Text = passGen.GeneratePassword(CheckWordProps(), -1, 0, (bool)chkb_randLetterSize.IsChecked, false, false, false, false);
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
            double passEntropy = passGen.CalculateEntropy(txtb_passTarget.Text);
            string strengthTxt = "";
            lbl_passStrength.Foreground = Brushes.White;

            if (passEntropy > 0.0)
            {
                strengthTxt = app.dataBank.GetLocalText("passWeak", cb_language.SelectedIndex);
                lbl_passStrength.Foreground = Brushes.Red;
            }

            if (passEntropy >= 65.0)
            {
                strengthTxt = app.dataBank.GetLocalText("passGood", cb_language.SelectedIndex);
                lbl_passStrength.Foreground = Brushes.Blue;
            }

            if (passEntropy >= 100.0)
            {
                strengthTxt = app.dataBank.GetLocalText("passGreat", cb_language.SelectedIndex);
                lbl_passStrength.Foreground = Brushes.Green;
            }

            lbl_passEntropy.Content = string.Format("{0}: {1} bit", app.dataBank.GetLocalText("entropy", cb_language.SelectedIndex), passEntropy);
            lbl_passStrength.Content = string.Format("{0}: {1}", app.dataBank.GetLocalText("passStrength", cb_language.SelectedIndex), strengthTxt);
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

        private void ApplyLocalizedUI()
        {
            // word options
            chkb_randWords.Content = app.dataBank.GetLocalText("words", cb_language.SelectedIndex);
            chkb_randWords.ToolTip = app.dataBank.GetToolTip("t_words", cb_language.SelectedIndex);

            lbl_wordCount.Content = string.Format("{0}:", app.dataBank.GetLocalText("wordCount", cb_language.SelectedIndex));
            chkb_randLetterSize.Content = app.dataBank.GetLocalText("randLetterSize", cb_language.SelectedIndex);

            // char options
            chkb_randChars.Content = app.dataBank.GetLocalText("characters", cb_language.SelectedIndex);
            chkb_randChars.ToolTip = app.dataBank.GetToolTip("t_characters", cb_language.SelectedIndex);
            lbl_charCount.Content = string.Format("{0}:", app.dataBank.GetLocalText("charCount", cb_language.SelectedIndex));
            chkb_randSpcChar.Content = string.Format("{0} (#@!/\\?[] etc.)", app.dataBank.GetLocalText("symbols", cb_language.SelectedIndex));

            rb_replaceWhiteChar.Content = app.dataBank.GetLocalText("replaceSpacing", cb_language.SelectedIndex);
            rb_replaceRandChar.Content = app.dataBank.GetLocalText("replaceChars", cb_language.SelectedIndex);
            rb_randPos.Content = app.dataBank.GetLocalText("randomPosition", cb_language.SelectedIndex);

            // password
            lbl_pass.Content = string.Format("{0}:", app.dataBank.GetLocalText("password", cb_language.SelectedIndex));
            UpdatePasswordStrength(null, null);

            // buttons
            btn_generate.Content = app.dataBank.GetLocalText("generate", cb_language.SelectedIndex);
            btn_generate.ToolTip = app.dataBank.GetToolTip("t_passGen", cb_language.SelectedIndex);
            btn_loadWordList.ToolTip = app.dataBank.GetToolTip("t_wordList", cb_language.SelectedIndex);
            btn_loadDefWordList.ToolTip = app.dataBank.GetToolTip("t_defWordList", cb_language.SelectedIndex);
        }

        private void SelectLanguage(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            app.fileO.SaveSettings("languageID", cb_language.SelectedIndex.ToString());
            ApplyLocalizedUI();
        }
    }
}
