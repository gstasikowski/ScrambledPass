namespace ScrambledPass.DesktopApp.Logic;

public class AppLogic
{
	private ScrambledPass.Models.GeneratorParameters _parameters;

	public Core CoreApp
	{
		get { return Core.Instance; }
	}

	public ScrambledPass.Models.GeneratorParameters Parameters
	{
		get { return _parameters; }
		set { _parameters = value; }
	}

	public static AppLogic Instance { get; set; } = new AppLogic();

	public AppLogic()
	{
		Settings.Instance.ApplyUserSettings();
		InitializeDefaultParameters();
	}

	private void InitializeDefaultParameters()
	{
		int initialWordCount = 5;
		int.TryParse(CoreApp.dataBank.GetSetting("defaultWordCount"), out initialWordCount);

		int initialSymbolCount = 5;
		int.TryParse(CoreApp.dataBank.GetSetting("defaultSymbolCount"), out initialSymbolCount);

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

	public void ToggleWordUse(bool enable)
	{
		int updatedWordCount = 0;

		if (enable)
		{
			int.TryParse(CoreApp.dataBank.GetSetting("defaultWordCount"), out updatedWordCount);
		}

		_parameters.WordCount = updatedWordCount;
	}

	public void ToggleSymbolUse(bool enable)
	{
		int updatedSymbolCount = 0;

		if (enable)
		{
			int.TryParse(CoreApp.dataBank.GetSetting("defaultSymbolCount"), out updatedSymbolCount);
		}

		_parameters.SymbolCount = updatedSymbolCount;
	}

	public void ChangeWordList(string filePath)
	{
		CoreApp.dataBank.SetSetting("lastWordList", filePath);
		CoreApp.fileOperations.PrepareWordList();
	}

	public void ResetWordList()
	{
		CoreApp.dataBank.SetSetting("lastWordList", string.Empty);
		CoreApp.fileOperations.PrepareWordList();
	}

	public string GeneratePassword()
	{
		return CoreApp.generator.GeneratePassword(_parameters);
	}

	public void RandomizeParameters()
	{
		_parameters.RandomizeParameters();
	}

	public double CalculatePasswordEntropy(string password)
	{
		return ScrambledPass.Logic.Helpers.CalculateEntropy(password, ref CoreApp.dataBank);
	}
}