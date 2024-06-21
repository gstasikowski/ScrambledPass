namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        public ErrorHandler(string errorCode) => new ErrorHandler(errorCode, string.Empty, null);

        public ErrorHandler(string errorCode, string? customMessage, Exception? innerException)
        {
            string errorMessage = string.Format("{0}/n{1}", errorCode, customMessage);
            throw new Exception(errorCode, innerException);
        }
    }
}
