using ScrambledPass.DesktopApp.Logic;

namespace ScrambledPass.DesktopApp.ViewModels;

public class GeneratorViewModel : ViewModelBase
{
	public AppLogic Generator
	{
		get { return AppLogic.Instance; }
	}

	public Localizer Lang
	{
		get { return Localizer.Instance; }
	}
}
