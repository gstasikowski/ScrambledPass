namespace ScrambledPass.DesktopApp.Controllers;

public class AppLogic
{
	private static Core _core = new Core();
	private ScrambledPass.Models.GeneratorParameters _parameters;

	public ScrambledPass.Models.GeneratorParameters Parameters
	{
		get { return _parameters; }
		set { _parameters = value; }
	}

	public int DefaultWordCount
	{
		get
		{
			int count = 1;
			int.TryParse(_core.dataBank.GetSetting("defaultWordCount"), out count);
			return count;
		}
		set { _core.dataBank.SetSetting("defaultWordCount", value.ToString()); }
	}

	public int DefaultSymbolCount
	{
		get
		{
			int count = 1;
			int.TryParse(_core.dataBank.GetSetting("defaultSymbolCount"), out count);
			return count;
		}
		set { _core.dataBank.SetSetting("defaultSymbolCount", value.ToString()); }
	}

	public AppLogic()
	{
		_core.fileOperations.LoadResources();
		InitializeDefaultParameters();
	}

	private void InitializeDefaultParameters()
	{
		int initialWordCount = 5;
		int.TryParse(_core.dataBank.GetSetting("defaultWordCount"), out initialWordCount);

		int initialSymbolCount = 5;
		int.TryParse(_core.dataBank.GetSetting("defaultSymbolCount"), out initialSymbolCount);

		_parameters = new ScrambledPass.Models.GeneratorParameters
		(
			wordCount: initialWordCount,
			symbolMode: 0,
			symbolCount: initialSymbolCount,
			randomizeLetterSize: true,
			useLetters: true,
			useCapitalLetters: true,
			useNumbers: true,
			useSymbols: true
		);
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

	public void RandomizeParameters()
	{
		_parameters.RandomizeParameters();
	}

	public double CalculatePasswordEntropy(string password)
	{
		return ScrambledPass.Logic.Helpers.CalculateEntropy(password, ref _core.dataBank);
	}

	public void SaveSettings()
	{
		_core.fileOperations.SaveSettings();
	}

	public void RestoreDefaultSettings()
	{
		_core.dataBank.SetDefaultSettings();
	}
}