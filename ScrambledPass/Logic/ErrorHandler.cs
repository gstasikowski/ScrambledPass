namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        public ErrorHandler(string errorCode)
        {
            View.ErrorMessage messageWindows = new View.ErrorMessage();
            Model.DataBank dataBase = new Model.DataBank();

            messageWindows.DisplayMessage(dataBase.GetErrorMessage(errorCode));
            messageWindows.Show();
        }
    }
}
