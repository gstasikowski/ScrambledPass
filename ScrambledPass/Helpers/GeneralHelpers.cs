using System;
using System.Threading.Tasks;

namespace ScrambledPass.Helpers
{
    class GeneralHelpers
    {
        public async Task ClearClipboardAsync()
        {
            if (Logic.Refs.dataBank.GetSetting("clearClipboard") == "False")
            { return; }

            int clearDelay = 15;

            int.TryParse(Logic.Refs.dataBank.GetSetting("clearClipboardDelay"), out clearDelay);

            await Task.Delay(TimeSpan.FromSeconds(clearDelay));
            System.Windows.Clipboard.Clear();
        }
    }
}
