namespace ScrambledPass.DesktopApp.ViewModels;

public class GeneratorViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
	public string EnableRandomWords => "Include random words";
	public string WordCount => "Word count";
	public string EnableRandomLetterSize => "Randomize letter size";
	public string EnableRandomCharacters => "Include random characters";
	public string UIReplaceSpacing => "Replace spacing";
	public string UIReplaceCharacters => "Replace random characters";
	public string UIRandomPosition => "Insert at random positions";
	public string UICharacterCount => "Character count";
	public string UIPassword => "Password";
	public string UIGenerate => "Generate";
	public string UIEntropy => "Entropy";
	public string Symbols => "Special characters";
	public string SymbolMode => "Symbol mode";
	public string UIPasswordStrength => "Password strength";
#pragma warning restore CA1822 // Mark members as static
}
