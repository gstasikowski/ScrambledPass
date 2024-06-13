using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScrambledPass.DesktopApp.ViewModels;

namespace ScrambledPass.DesktopApp.Views;

public partial class SettingsView : UserControl
{
	public SettingsView()
	{
		InitializeComponent();
	}

	private void SaveSettings(object sender, RoutedEventArgs e)
	{
		((SettingsViewModel)this.DataContext).Generator.SaveSettings();
	}

	private void RestoreDefaultSettings(object sender, RoutedEventArgs e)
	{
		((SettingsViewModel)this.DataContext).Generator.RestoreDefaultSettings();
	}

	private void CloseSettingsView(object sender, RoutedEventArgs e)
	{
		// return to generator view
	}

	private void ChangeLanguage(object sender, SelectionChangedEventArgs e)
	{
		string languageCode = ((ComboBox)sender).SelectedItem.ToString();
		((SettingsViewModel)this.DataContext).Generator.SetSetting("languageID", languageCode);
	}
}