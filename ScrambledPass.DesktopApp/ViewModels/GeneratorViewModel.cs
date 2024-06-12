using ScrambledPass.DesktopApp.Controllers;

namespace ScrambledPass.DesktopApp.ViewModels;

public class GeneratorViewModel : ViewModelBase
{
	private AppLogic? _appLogic;

	public AppLogic? Generator
	{
		get { return _appLogic; }
	}

	public GeneratorViewModel(AppLogic sourceController)
	{
		_appLogic = sourceController;
	}
}
