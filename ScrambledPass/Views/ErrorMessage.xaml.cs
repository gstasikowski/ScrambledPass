using System.Windows;

namespace ScrambledPass.Views
{
    /// <summary>
    /// Interaction logic for ErroMessage.xaml
    /// </summary>
    public partial class ErrorMessage : Window
    {
        public ErrorMessage()
        {
            InitializeComponent();
        }

        public void DisplayMessage(string message)
        {
            Application.Current.MainWindow.IsEnabled = false;
            lbl_message.Content = message;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.MainWindow.IsEnabled = true;
        }
    }
}
