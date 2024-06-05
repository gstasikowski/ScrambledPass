using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ScrambledPass.DesktopApp.ViewModels;
using ScrambledPass.DesktopApp.Views;

namespace ScrambledPass.DesktopApp;

public partial class App : Application
{

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// desktop.MainWindow = new MainWindow
			// {
			//     DataContext = new MainWindowViewModel(),
			// };

			desktop.MainWindow = new GeneratorView
			{
				DataContext = new GeneratorViewModel(),
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}