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

            Views.ErrorMessage messageWindow = new Views.ErrorMessage(message);
            messageWindow.Show();
        }
    }
}
