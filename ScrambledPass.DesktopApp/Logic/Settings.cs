using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ScrambledPass.DesktopApp.Logic;

public class Settings : INotifyPropertyChanged
{
	private Core CoreApp
	{
		get { return Core.Instance; }
	}

	public static Settings Instance { get; set; } = new Settings();
	public event PropertyChangedEventHandler PropertyChanged;

	public int SelectedLanguage
	{
		get { return Localizer.Instance.SelectedLanguageIndex(); }
		set
		{
			Localizer.Instance.LoadLanguage(Localizer.Instance.LanguageList[value]);
			SetSetting("languageID", Localizer.Instance.LanguageList[value]);
		}
	}

	public int SelectedTheme
	{
		get { return GetThemeIndex(); }
		set { ChangeTheme(value); }
	}

	public bool RememberLastWordList
	{
		get { return GetSetting("rememberLastWordList") == "True"; }
		set { ToggleLastWordList(value); }
	}

	public int DefaultWordCount
	{
		get { return HelperMethods.ParseIntSetting("defaultWordCount"); }
		set { SetSetting("defaultWordCount", value.ToString()); }
	}

	public int DefaultSymbolCount
	{
		get { return HelperMethods.ParseIntSetting("defaultSymbolCount"); }
		set { SetSetting("defaultSymbolCount", value.ToString()); }
	}

	public bool ClearClipboard
	{
		get { return GetSetting("clearClipboard") == "True"; }
		set { SetSetting("clearClipboard", value.ToString()); }
	}

	public int ClearClipboardDelay
	{
		get { return HelperMethods.ParseIntSetting("clearClipboardDelay"); }
		set { SetSetting("clearClipboardDelay", value.ToString()); }
	}

	public void ApplyUserSettings()
	{
		Localizer.Instance.LoadLanguage(GetSetting("languageID"));

		int themeIndex = 0;
		int.TryParse(GetSetting("theme"), out themeIndex);
		SelectedTheme = themeIndex;
	}

	public void SaveSettings()
	{
		CoreApp.fileOperations.SaveSettings();
	}

	public void RestoreDefaultSettings()
	{
		CoreApp.fileOperations.LoadDefaultSettings(CoreApp.dataBank.DefaultConfigFile);
		Localizer.Instance.LoadLanguage(GetSetting("languageID"));
		OnPropertyChanged("SelectedLanguage");
		OnPropertyChanged("DefaultWordCount");
		OnPropertyChanged("DefaultSymbolCount");
	}

	public string GetSetting(string settingID)
	{
		return CoreApp.dataBank.GetSetting(settingID);
	}

	public void SetSetting(string settingID, string value)
	{
		CoreApp.dataBank.SetSetting(settingID, value);
	}

	protected void OnPropertyChanged([CallerMemberName] string name = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}

	private void ChangeTheme(int index)
	{
		switch (index)
		{
			case 1:
				Avalonia.Application.Current.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Light;
				break;

			case 2:
				Avalonia.Application.Current.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
				break;

			default:
				Avalonia.Application.Current.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Default;
				break;
		}

		SetSetting("theme", index.ToString());
	}

	private int GetThemeIndex()
	{
		switch (Avalonia.Application.Current.RequestedThemeVariant.ToString())
		{
			case "Light":
				return 1;

			case "Dark":
				return 2;

			default:
				return 0;
		}
	}

	private void ToggleLastWordList(bool enable)
	{
		SetSetting("rememberLastWordList", enable.ToString());

		if (!enable)
		{
			SetSetting("lastWordList", string.Empty);
		}
	}
}