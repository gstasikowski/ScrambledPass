using System.Windows;

namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        public ErrorHandler(string errorCode) => new ErrorHandler(errorCode, string.Empty);

        public ErrorHandler(string errorCode, string exceptionMessage)
        {
            Application app = Application.Current;
            string message = (exceptionMessage != string.Empty) ? string.Format("\n\nException message: {0}", exceptionMessage) : exceptionMessage;

            try
            { message = (string)app.FindResource(errorCode) + message; }
            catch
            { message = (string)app.FindResource("ErrorDefault") + message; }

            Application.Current.MainWindow.IsEnabled = false;
            View.ErrorMessage messageWindows = new View.ErrorMessage(message);
            messageWindows.Show();
        }
    }
}
