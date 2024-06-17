using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using ScrambledPass.Models;

namespace ScrambledPass.Logic
{
	public class FileOperations
	{
		private DataBank _dataBank;

		public FileOperations(DataBank dataBank)
		{
			_dataBank = dataBank;
			LoadResources();
		}

		public void LoadResources()
		{
			LoadSettings();
			PrepareWordList();
		}

		public void PrepareWordList()
		{
			string wordlistFilePath = _dataBank.GetSetting("lastWordList");

			_dataBank.WordList.Clear();

			if (wordlistFilePath == string.Empty)
			{
				_dataBank.WordList.AddRange(LoadDefaultWordList());
			}
			else
			{
				_dataBank.WordList.AddRange(LoadCustomWordList(wordlistFilePath));
			}
		}

		public List<string> LoadDefaultWordList()
		{
			List<string> wordList = new List<string>();
			try
			{
				var assembly = Assembly.GetExecutingAssembly();

				using (Stream stream = assembly.GetManifestResourceStream(_dataBank.DefaultWordListFile))
				using (StreamReader reader = new StreamReader(stream))
				{
					string? newWord = reader.ReadLine();

					while (!string.IsNullOrEmpty(newWord))
					{
						wordList.Add(newWord);
						newWord = reader.ReadLine();
					}

					reader.Close();
				}
			}
			catch (FileNotFoundException e)
			{
				new ErrorHandler("ErrorFileNotFound", null, e.InnerException);
			}

			_dataBank.SetSetting("lastWordList", string.Empty);
			return wordList;
		}

		public List<string> LoadCustomWordList(string filePath)
		{
			List<string> wordList = new List<string>();

			if (File.Exists(filePath))
			{
				wordList.Clear();
				wordList.AddRange(File.ReadAllLines(filePath));

				_dataBank.SetSetting("lastWordList", filePath);
			}
			else
			{
				new ErrorHandler("ErrorFileNotFound");
			}

			return wordList;
		}

		public void LoadSettings()
		{
			string configFilePath = _dataBank.ConfigFile;

			if (File.Exists(configFilePath))
			{
				string configFile = File.ReadAllText(configFilePath);
				ParseSettings(configFile);
			}
			else
			{
				LoadDefaultSettings(_dataBank.DefaultConfigFile);
				SaveSettings();
			}
		}

		public void LoadDefaultSettings(string assemblyConfigFile)
		{
			try
			{
				var assembly = Assembly.GetExecutingAssembly();

				using (Stream stream = assembly.GetManifestResourceStream(assemblyConfigFile))
				using (StreamReader reader = new StreamReader(stream))
				{
					string? configContent = reader.ReadToEnd();
					ParseSettings(configContent);

					reader.Close();
				}
			}
			catch (FileNotFoundException e)
			{
				new ErrorHandler("ErrorFileNotFound", null, e.InnerException);
			}
		}

		public void SaveSettings()
		{
			Dictionary<string, string> appSettings = _dataBank.GetAllSettings();

			FileStream fileStream;
			fileStream = new FileStream(_dataBank.ConfigFile, FileMode.Create);

			XElement rootElement = new XElement("Config", appSettings.Select(kv => new XElement(kv.Key, kv.Value)));
			XmlSerializer serializer = new XmlSerializer(rootElement.GetType());
			serializer.Serialize(fileStream, rootElement);

			fileStream.Close();
		}

		public void ParseSettings(string configContent)
		{
			XElement rootElement = XElement.Parse(configContent);

			foreach (var element in rootElement.Elements())
			{
				_dataBank.SetSetting(element.Name.LocalName, element.Value);
			}
		}
	}
}
