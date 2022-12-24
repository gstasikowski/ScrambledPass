namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        public ErrorHandler(string errorCode) => new ErrorHandler(errorCode, string.Empty);

        public ErrorHandler(string errorCode, string exceptionMessage)
        {
            throw new Exception(errorCode);
        }
    }
}
