using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScrambledPass.DesktopApp.ViewModels;

namespace ScrambledPass.DesktopApp.Views;

public partial class GeneratorView : Window
{
	private static Core _core = new Core();

	public GeneratorView()
	{
		InitializeComponent();
		InitializeCoreApp();
		SubscribeToEvents();
	}

	private void InitializeCoreApp()
	{
		_core.fileOperations.LoadResources();
	}

	private void SubscribeToEvents()
	{
		Password.TextChanged += UpdatePasswordStrength;
	}

	private void ToggleWordPanel(object sender, RoutedEventArgs e)
	{
		WordPanel.IsVisible = GetCheckBoxState(sender as CheckBox);
		WordCount.IsEnabled = WordPanel.IsVisible;
		ToggleSymbolModeSelection(WordPanel.IsVisible);
	}

	private void ToggleSymbolModeSelection(bool enable)
	{
		SymbolMode.IsEnabled = enable;

		if (!enable)
		{
			SymbolMode.SelectedIndex = 0;
		}
	}

	private void ToggleSymbolPanel(object sender, RoutedEventArgs e)
	{
		CharacterPanel.IsVisible = GetCheckBoxState(sender as CheckBox);
		CharacterCount.IsEnabled = CharacterPanel.IsVisible;
	}

	private void GeneratePassword(object sender, RoutedEventArgs e)
	{
		string newPassword = _core.generator.GeneratePassword(
			wordCount: GetWordCount(),
			symbolMode: SymbolMode.SelectedIndex,
			symbolCount: UseRandomSymbols() ? GetCharacterCount() : 0,
			randomCharacterSize: GetCheckBoxState(EnableRandomLetterSize),
			useLetters: GetCheckBoxState(EnableRandomLowerLetters),
			useCapitalLetters: GetCheckBoxState(EnableRandomUpperLetters),
			useNumbers: GetCheckBoxState(EnableRandomNumbers),
			useSymbols: GetCheckBoxState(EnableSymbols)
		);

		Password.Text = newPassword;
	}

	private int GetWordCount()
	{
		if (!EnableRandomWords.IsChecked ?? false)
		{
			return 0;
		}

		return ValidateNumberValue(WordCount.Text);
	}

	private int GetCharacterCount()
	{
		return ValidateNumberValue(CharacterCount.Text);
	}

	private int ValidateNumberValue(string? sourceText)
	{
		int numberValue = 0;
		int.TryParse(sourceText, out numberValue);
		return numberValue;
	}

	private bool GetCheckBoxState(CheckBox source)
	{
		return source.IsChecked ?? false;
	}

	private bool UseRandomSymbols()
	{
		return EnableRandomCharacters.IsChecked ?? false;
	}

	private void UpdatePasswordStrength(object sender, TextChangedEventArgs e)
	{
		string strengthTxt = "";
		double passwordEntropy = Logic.Helpers.CalculateEntropy((sender as TextBox).Text, ref _core.dataBank);
		PasswordEntropy.Content = $"{((GeneratorViewModel)this.DataContext).UIEntropy}: {passwordEntropy}";

		if (passwordEntropy > 0.0)
		{
			strengthTxt = "weak";
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Red"));
		}

		if (passwordEntropy >= 65.0)
		{
			strengthTxt = "good";
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Yellow"));

		}

		if (passwordEntropy >= 100.0)
		{
			strengthTxt = "great";
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Green"));

		}

		PasswordStrength.Content = $"{((GeneratorViewModel)this.DataContext).UIPasswordStrength}: {strengthTxt}";
	}
}