using System.Collections.ObjectModel;

namespace ScrambledPass.Models
{
	public class DataBank
	{
		#region Variables
		private readonly static string _defaultWordListFile = "ScrambledPass.defaultWordList.txt";
		private readonly string _defaultConfigFile = "ScrambledPass.defaultConfig.xml";
		private readonly string _configFile = AppDomain.CurrentDomain.BaseDirectory + "Config.xml";
		private readonly string _defaultThemePath = AppDomain.CurrentDomain.BaseDirectory + "Themes\\";

		private Dictionary<string, string> _settings = new Dictionary<string, string>();
		private List<string> _wordList = new List<string>();
		private static readonly char[] _symbols = { ' ', '.', ',', ';', '/', '\\', '\'', '[', ']', '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '|', ':', '\"', '<', '>', '?' };

		private ObservableCollection<string> _themes = new ObservableCollection<string>();
		#endregion Variables

		#region Properties
		public string AppPath
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
		}

		public string DefaultWordListFile
		{
			get { return _defaultWordListFile; }
		}

		public string DefaultConfigFile
		{
			get { return _defaultConfigFile; }
		}

		public string ConfigFile
		{
			get { return _configFile; }
		}

		public string DefaultThemePath
		{
			get { return _defaultThemePath; }
		}

		public List<string> WordList
		{
			get { return _wordList; }
		}

		public char[] Symbols
		{
			get { return _symbols; }
		}

		public int SpecialCharsCount
		{
			get { return _symbols.Length; }
		}

		public ObservableCollection<string> ThemeList
		{
			get { return _themes; }
			set { _themes = value; }
		}
		#endregion Properties

		#region Methods
		public Dictionary<string, string> GetAllSettings()
		{
			return _settings;
		}

		public string GetSetting(string key)
		{
			return _settings[key];
		}

		public void SetSetting(string key, string value)
		{
			if (_settings.ContainsKey(key))
			{
				_settings[key] = value;
			}
			else
			{
				_settings.Add(key, value);
			}
		}

		public void AddAvailableTheme(string themeName)
		{
			if (!_themes.Contains(themeName))
			{
				_themes.Add(themeName);
			}
		}

		public int ThemeIndex(string themeName)
		{
			int themeIndex = _themes.IndexOf(themeName);
			return (themeIndex > -1) ? themeIndex : 0;
		}
		#endregion Methods
	}
}
