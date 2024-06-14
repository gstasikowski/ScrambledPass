using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ScrambledPass.DesktopApp.ViewModels;

namespace ScrambledPass.DesktopApp.Views;

public partial class GeneratorView : UserControl
{
	public GeneratorView()
	{
		InitializeComponent();
		SubscribeToEvents();
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
		((GeneratorViewModel)this.DataContext).Generator.ToggleWordUse(WordPanel.IsVisible);
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
		((GeneratorViewModel)this.DataContext).Generator.ToggleSymbolUse(CharacterPanel.IsVisible);
	}

	private void GeneratePassword(object sender, RoutedEventArgs e)
	{
		Password.Text = ((GeneratorViewModel)this.DataContext).Generator.GeneratePassword();
	}

	private void RandomizeParameters(object sender, RoutedEventArgs e)
	{
		((GeneratorViewModel)this.DataContext).Generator.RandomizeParameters();
	}

	private bool GetCheckBoxState(CheckBox source)
	{
		return source.IsChecked ?? false;
	}

	private void UpdatePasswordStrength(object sender, TextChangedEventArgs e)
	{
		string strengthTxt = "";
		double passwordEntropy = ((GeneratorViewModel)this.DataContext).Generator.CalculatePasswordEntropy((sender as TextBox).Text);
		PasswordEntropy.Content = $"{((GeneratorViewModel)this.DataContext).Lang["Entropy"]}: {passwordEntropy}";
		object fontColor = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("White"));

		FindPasswordStrengthColor(passwordEntropy, ref strengthTxt, ref fontColor);

		PasswordStrength.Foreground = (Avalonia.Media.SolidColorBrush)fontColor;
		PasswordStrength.Content = $"{((GeneratorViewModel)this.DataContext).Lang["PasswordStrength"]}: {strengthTxt}";
	}

	private void FindPasswordStrengthColor(double passwordEntropy, ref string strengthTxt, ref object fontColor)
	{
		if (passwordEntropy >= 100.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).Lang["PasswordGreat"];
			this.TryFindResource("PasswordQualityGreat", this.ActualThemeVariant, out fontColor);
			return;
		}

		if (passwordEntropy >= 75.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).Lang["PasswordGood"];
			this.TryFindResource("PasswordQualityGood", this.ActualThemeVariant, out fontColor);
			return;
		}

		if (passwordEntropy >= 40.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).Lang["PasswordWeak"];
			this.TryFindResource("PasswordQualityWeak", this.ActualThemeVariant, out fontColor);
			return;
		}

		strengthTxt = ((GeneratorViewModel)this.DataContext).Lang["PasswordPoor"];
		this.TryFindResource("PasswordQualityPoor", this.ActualThemeVariant, out fontColor);
	}

	private async void OpenFilePicker(object sender, RoutedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(this);

		var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			Title = ((GeneratorViewModel)this.DataContext).Lang["SelectWordFile"],
			AllowMultiple = false,
			FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
		});

		if (files.Count >= 1)
		{
			string filePath = files[0].Path.ToString().Replace("file://", string.Empty);
			((GeneratorViewModel)this.DataContext).Generator.ChangeWordList(filePath);
		}
	}

	private void ResetWordList(object sender, RoutedEventArgs e)
	{
		((GeneratorViewModel)this.DataContext).Generator.ResetWordList();
	}
}