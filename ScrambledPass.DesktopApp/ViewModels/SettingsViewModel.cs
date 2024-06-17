using System.Collections.ObjectModel;
using ScrambledPass.DesktopApp.Logic;

namespace ScrambledPass.DesktopApp.ViewModels;

public class SettingsViewModel : ViewModelBase
{
	public AppLogic Generator
	{
		get { return AppLogic.Instance; }
	}

	public Settings AppSettings
	{
		get { return Settings.Instance; }
	}

	public ObservableCollection<string> LanguageList
	{
		get { return Localizer.Instance.LanguageList; }
	}
}
