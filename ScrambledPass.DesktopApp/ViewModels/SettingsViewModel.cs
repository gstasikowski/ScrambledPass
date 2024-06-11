using ScrambledPass.DesktopApp.Controllers;

namespace ScrambledPass.DesktopApp.ViewModels;

public class SettingsViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
	public string UILanguage => "Language";
	public string UITheme => "Theme";
	public string UILoadWordList => "Load last used wordlist on startup";
	public string UIDefWordCount => "Default word count";
	public string UIDefCharacterCount => "Default character count";
	public string UIClearClipboard => "Clear clipboard";
	public string UIClearClipboardDelay => "Clearing delay";
	public string UISave => "Save";
	public string UIRestoreDefault => "Restore default";
	public string UIBack => "Return";
#pragma warning restore CA1822 // Mark members as static

	private AppLogic? _appLogic;

	public AppLogic? Generator
	{
		get { return _appLogic; }
	}

	public SettingsViewModel(AppLogic sourceController)
	{
		_appLogic = sourceController;
	}
}
