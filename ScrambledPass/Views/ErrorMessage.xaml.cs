using System.Windows;

namespace ScrambledPass.Views
{
    /// <summary>
    /// Interaction logic for ErroMessage.xaml
    /// </summary>
    public partial class ErrorMessage : Window
    {
        public ErrorMessage(string message)
        {
            InitializeComponent();
            lblMessage.Content = message;
            Application.Current.MainWindow.IsEnabled = false;
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
