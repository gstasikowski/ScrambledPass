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
	private int _symbolMode = 0;

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
		txtPassword.TextChanged += UpdatePasswordStrength;
	}

	private void ToggleWordPanel(object sender, RoutedEventArgs e)
	{
		WordsPanel.IsVisible = GetCheckBoxState(sender as CheckBox);
	}

	private void ToggleSymbolPanel(object sender, RoutedEventArgs e)
	{
		pnlChars.IsVisible = GetCheckBoxState(sender as CheckBox);
	}

	private void SetSymbolMode(object sender, RoutedEventArgs e)
	{
		_symbolMode = ValidateNumberValue((sender as Avalonia.Controls.RadioButton).Tag.ToString());
	}

	private void GeneratePassword(object sender, RoutedEventArgs e)
	{
		Debug.WriteLine("\nHere's your new password:");
		string newPassword = _core.generator.GeneratePassword(
			wordCount: GetWordCount(),
			symbolMode: _symbolMode,
			symbolCount: GetCharacterCount(),
			randomCharacterSize: false,
			useLetters: GetCheckBoxState(chkRandomLower),
			useCapitalLetters: GetCheckBoxState(chkRandomUpper),
			useNumbers: GetCheckBoxState(chkRandomNumbers),
			useSymbols: GetCheckBoxState(chkRandomSymbols)
		);

		Debug.WriteLine(newPassword);
		txtPassword.Text = newPassword;
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
		return ValidateNumberValue(txtCharCount.Text);
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

	private void UpdatePasswordStrength(object sender, TextChangedEventArgs e)
	{
		string strengthTxt = "";
		double passwordEntropy = Logic.Helpers.CalculateEntropy((sender as TextBox).Text, ref _core.dataBank);
		lblPasswordEntropy.Content = $"{((GeneratorViewModel)this.DataContext).UIEntropy}: {passwordEntropy}";

		if (passwordEntropy > 0.0)
		{
			strengthTxt = "weak";
			lblPasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Red"));
		}

		if (passwordEntropy >= 65.0)
		{
			strengthTxt = "good";
			lblPasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Yellow"));

		}

		if (passwordEntropy >= 100.0)
		{
			strengthTxt = "great";
			lblPasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Green"));

		}

		lblPasswordStrength.Content = $"{((GeneratorViewModel)this.DataContext).UIPasswordStrength}: {strengthTxt}";
	}
}