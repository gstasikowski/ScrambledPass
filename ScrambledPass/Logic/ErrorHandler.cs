namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        public ErrorHandler(string errorCode)
        {
            App app = (App)App.Current;
            View.ErrorMessage messageWindows = new View.ErrorMessage();

            string message = app.dataBank.GetErrorMessage(errorCode, int.Parse(Properties.Settings.Default["languageID"].ToString()));

            messageWindows.DisplayMessage(message);
            messageWindows.Show();
        }
    }
}
