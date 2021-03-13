namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        public ErrorHandler(string errorCode)
        {
            App app = (App)App.Current;
            View.ErrorMessage messageWindows = new View.ErrorMessage();

            messageWindows.DisplayMessage(app.dataBank.GetErrorMessage(errorCode, (int)Properties.Settings.Default["languageID"]));
            messageWindows.Show();
        }
    }
}
