using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.Json;
using Avalonia.Platform;

namespace ScrambledPass.DesktopApp.Logic
{
	public class Localizer : INotifyPropertyChanged
	{
		private ObservableCollection<string> _languages = new ObservableCollection<string>();
		private const string IndexerName = "Item";
		private const string IndexerArrayName = "Item[]";
		private Dictionary<string, string> m_Strings = null;

		public ObservableCollection<string> LanguageList
		{
			get { return _languages; }
			set { _languages = value; }
		}

		public Localizer()
		{
			Uri localizationFolder = new Uri("avares://ScrambledPass.DesktopApp/Assets/Localization");
			var fileList = AssetLoader.GetAssets(localizationFolder, new Uri("avares://ScrambledPass.DesktopApp/Assets"));

			foreach (Uri file in fileList)
			{
				AddAvailableLanguage(file.ToString().Split("/")[^1].Replace(".json", string.Empty));
			}
		}

		public void AddAvailableLanguage(string languageCode)
		{
			if (!LanguageList.Contains(languageCode))
			{
				LanguageList.Add(languageCode);
			}
		}

		public bool LoadLanguage(string language)
		{
			Language = language;

			Uri uri = new Uri($"avares://ScrambledPass.DesktopApp/Assets/Localization/{language}.json");
			if (AssetLoader.Exists(uri))
			{
				using (StreamReader sr = new StreamReader(AssetLoader.Open(uri), Encoding.UTF8))
				{
					m_Strings = JsonSerializer.Deserialize<Dictionary<string, string>>(sr.ReadToEnd());
				}
				Invalidate();

				return true;
			}

			return false;
		}

		public string Language { get; private set; }

		public string this[string key]
		{
			get
			{
				string res;
				if (m_Strings != null && m_Strings.TryGetValue(key, out res))
					return res.Replace("\\n", "\n");

				return $"{Language}:{key}";
			}
		}

		public static Localizer Instance { get; set; } = new Localizer();
		public event PropertyChangedEventHandler PropertyChanged;

		public void Invalidate()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
		}

		public int SelectedLanguageIndex()
		{
			return LanguageList.IndexOf(Language);
		}

		// Create the OnPropertyChanged method to raise the event
		// The calling member's name will be used as the parameter.
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}