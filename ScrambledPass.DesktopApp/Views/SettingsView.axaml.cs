using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScrambledPass.DesktopApp.ViewModels;

namespace ScrambledPass.DesktopApp.Views
{
	public partial class SettingsView : UserControl
	{
		public SettingsView()
		{
			InitializeComponent();
		}

		private void SaveSettings(object sender, RoutedEventArgs e)
		{
			((SettingsViewModel)this.DataContext).AppSettings.SaveSettings();
		}

		private void RestoreDefaultSettings(object sender, RoutedEventArgs e)
		{
			((SettingsViewModel)this.DataContext).AppSettings.RestoreDefaultSettings();

		}
	}
}