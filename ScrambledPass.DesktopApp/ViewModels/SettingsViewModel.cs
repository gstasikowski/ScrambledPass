using System.Collections.ObjectModel;
using ScrambledPass.DesktopApp.Logic;

namespace ScrambledPass.DesktopApp.ViewModels;

public class SettingsViewModel : ViewModelBase
{
	public AppLogic Generator
	{
		get { return AppLogic.Instance; }
	}

	public ObservableCollection<string> LanguageList
	{
		get { return Localizer.Instance.LanguageList; }
	}

	public int SelectedLanguage
	{
		get { return Localizer.Instance.SelectedLanguageIndex(); }
		set { Localizer.Instance.LoadLanguage(Localizer.Instance.LanguageList[value]); }
	}
}
