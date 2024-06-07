namespace ScrambledPass.DesktopApp.Controllers;

public class GeneratorController
{
	private static Core _core = new Core();
	private ScrambledPass.Models.GeneratorParameters _parameters = new ScrambledPass.Models.GeneratorParameters();

	public ScrambledPass.Models.GeneratorParameters Parameters
	{
		get { return _parameters; }
		set { _parameters = value; }
	}

	public GeneratorController()
	{
		_core.fileOperations.LoadResources();
	}

	public void ChangeWordList(string filePath)
	{
		_core.dataBank.SetSetting("lastWordList", filePath);
		_core.fileOperations.PrepareWordList();
	}

	public void ResetWordList()
	{
		_core.dataBank.SetSetting("lastWordList", string.Empty);
		_core.fileOperations.PrepareWordList();
	}

	public string GeneratePassword()
	{
		return _core.generator.GeneratePassword(_parameters);
	}

	public double CalculatePasswordEntropy(string password)
	{
		return Logic.Helpers.CalculateEntropy(password, ref _core.dataBank);
	}
}