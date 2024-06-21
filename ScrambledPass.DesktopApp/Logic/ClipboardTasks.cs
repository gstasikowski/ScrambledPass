using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScrambledPass.DesktopApp.Logic
{
	public class ClipboardTasks
	{
		private CancellationTokenSource _tokenSource;

		public static ClipboardTasks Instance { get; set; } = new ClipboardTasks();

		public void CancelClipboardClearingTask()
		{
			if (_tokenSource != null)
			{
				_tokenSource?.Cancel();
				_tokenSource?.Dispose();
			}
		}

		public async Task ClearClipboardAsync(Avalonia.Controls.TopLevel window)
		{
			if (Core.Instance.dataBank.GetSetting("clearClipboard") == "False" || window == null)
			{ return; }

			_tokenSource = new CancellationTokenSource();
			CancellationToken ct = _tokenSource.Token;

			int clearDelay = 15;
			int.TryParse(Core.Instance.dataBank.GetSetting("clearClipboardDelay"), out clearDelay);
			await Task.Delay(TimeSpan.FromSeconds(clearDelay));

			var task = Task.Run(async () =>
			{
				ct.ThrowIfCancellationRequested();

				await window?.Clipboard.ClearAsync();
			}, _tokenSource.Token);

			try
			{
				await task;
			}
			catch (OperationCanceledException e)
			{ }
			finally
			{
				_tokenSource.Dispose();
				_tokenSource = new CancellationTokenSource();
			}
		}
	}
}