using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ScrambledPass.DesktopApp.Logic;

public class Settings : INotifyPropertyChanged
{
	private Core CoreApp
	{
		get { return AppLogic.Instance.CoreApp; }
	}

	public static Settings Instance { get; set; } = new Settings();
	public event PropertyChangedEventHandler PropertyChanged;


	public int SelectedLanguage
	{
		get { return Localizer.Instance.SelectedLanguageIndex(); }
		set { Localizer.Instance.LoadLanguage(Localizer.Instance.LanguageList[value]); }
	}

	public int DefaultWordCount
	{
		get
		{
			int count = 1;
			int.TryParse(CoreApp.dataBank.GetSetting("defaultWordCount"), out count);
			return count;
		}
		set { CoreApp.dataBank.SetSetting("defaultWordCount", value.ToString()); }
	}

	public int DefaultSymbolCount
	{
		get
		{
			int count = 1;
			int.TryParse(CoreApp.dataBank.GetSetting("defaultSymbolCount"), out count);
			return count;
		}
		set { CoreApp.dataBank.SetSetting("defaultSymbolCount", value.ToString()); }
	}

	public void SaveSettings()
	{
		CoreApp.fileOperations.SaveSettings();
	}

	public void RestoreDefaultSettings()
	{
		CoreApp.fileOperations.LoadDefaultSettings(CoreApp.dataBank.DefaultConfigFile);
		Localizer.Instance.LoadLanguage(CoreApp.dataBank.GetSetting("languageID"));
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
}