using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrambledPass.Logic
{
    public class ErrorHandler
    {
        Dictionary<string, string> errorValues = new Dictionary<string, string> {
            { "default", "Unkown error has occured. Please try again." },
            { "parserError", "Word and character count fields require numerical input.\nPlease fix problematic values and try again." },
            { "fileNotFound", "Word list file can't be accessed." }
        };

        public ErrorHandler(string errorCode)
        {
            Views.ErrorMessage messageWindows = new Views.ErrorMessage();

            try
            { messageWindows.DisplayMessage(errorValues[errorCode]); }
            catch
            { messageWindows.DisplayMessage(errorValues["default"]); }

            messageWindows.Show();
        }
    }
}
