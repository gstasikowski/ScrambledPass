using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ScrambledPass.DesktopApp.Controllers;
using ScrambledPass.DesktopApp.ViewModels;

namespace ScrambledPass.DesktopApp.Views;

public partial class GeneratorView : Window
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
		PasswordEntropy.Content = $"{((GeneratorViewModel)this.DataContext).UIEntropy}: {passwordEntropy}";

		if (passwordEntropy > 0.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).PasswordPoor;
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Red"));
		}

		if (passwordEntropy >= 40.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).PasswordWeak;
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Yellow"));
		}

		if (passwordEntropy >= 75.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).PasswordGood;
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Blue"));
		}

		if (passwordEntropy >= 100.0)
		{
			strengthTxt = ((GeneratorViewModel)this.DataContext).PasswordGreat;
			PasswordStrength.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("Green"));
		}

		PasswordStrength.Content = $"{((GeneratorViewModel)this.DataContext).UIPasswordStrength}: {strengthTxt}";
	}

	private async void OpenFilePicker(object sender, RoutedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(this);

		var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			Title = "Open Text File",
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