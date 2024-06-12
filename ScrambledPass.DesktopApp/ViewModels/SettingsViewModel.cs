using System.Collections.ObjectModel;
using ScrambledPass.DesktopApp.Controllers;

namespace ScrambledPass.DesktopApp.ViewModels;

public class SettingsViewModel : ViewModelBase
{
	private AppLogic? _appLogic;

	public AppLogic? Generator
	{
		get { return _appLogic; }
	}

	public SettingsViewModel(AppLogic sourceController)
	{
		_appLogic = sourceController;
	}

	public ObservableCollection<string> LanguageList
	{
		get { return Logic.Localizer.Instance.LanguageList; }
	}
}
