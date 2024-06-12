namespace ScrambledPass.ConsoleApp
{
	class Program
	{
		private static Core _core = new Core();
		private static ScrambledPass.Models.GeneratorParameters settings = new ScrambledPass.Models.GeneratorParameters();

		public static void Main(string[] args)
		{
			_core.fileOperations.LoadResources();
			DisplayMainMenu();
		}

		private static void DisplayMainMenu()
		{
			PrintGeneratorSettings();
			PrintMenuOptions();
		}

		private static void DisplayGeneratorMenu()
		{
			PrintGeneratorSettings();
			PrintGeneratorOptions();
		}

		private static void PrintGeneratorSettings()
		{
			Console.Clear();
			Console.WriteLine("Current settings\n――――――――――――――――");
			Console.WriteLine(
				"{0, -35} {1, -35} {2, -35}",
				$"Word count: {settings.WordCount} ",
				$"⏐ Symbol count: {settings.SymbolCount}",
				$"⏐ Symbol mode: {(Helpers.SymbolMode)settings.SymbolMode}"
			);
			System.Console.WriteLine(
				"{0, -35} {1, -35} {2, -35}",
				$"Randomize letter size: {settings.RandomizeLetterSize}",
				$"⏐ Use random letters: {settings.UseLetters}",
				$"⏐ Use random capital letters: {settings.UseCapitalLetters}"
			);
			System.Console.WriteLine(
				"{0, -35} {1, -35}",
				$"Use random numbers: {settings.UseNumbers}",
				$"⏐ Use random symbols: {settings.UseSymbols}\n"
			);
		}
		private static void PrintMenuOptions()
		{
			Console.WriteLine("What do you want to do?\n[1] Generate password\n[2] Open generator settings\n[3] Check password entropy\n[0] Exit");
			Console.Write("\n> ");
			string? option = Console.ReadLine();

			switch (option)
			{
				case "1":
					Console.WriteLine("\nHere's your new password:");
					string newPassword = _core.generator.GeneratePassword(settings);

					Console.WriteLine(newPassword);
					DisplayPasswordEntropy(newPassword);
					Console.ReadKey();
					break;

				case "2":
					DisplayGeneratorMenu();
					break;

				case "3":
					Console.WriteLine("Enter password to rate:");
					string testPassword = Console.ReadLine();
					DisplayPasswordEntropy(testPassword);
					break;

				case "0":
					System.Environment.Exit(1);
					break;

				default:
					break;
			}

			DisplayMainMenu();
		}

		private static void PrintGeneratorOptions()
		{
			Console.WriteLine("What do you want to do?\n[1] Set word count\n[2] Set symbol count\n[3] Set symbol mode\n[4] Toggle letter size randomization\n[5] Toggle random letters\n[6] Toggle captial letters\n[7] Toggle numbers\n[8] Toggle symbols\n[9] Randomize settings\n[0] Return");
			Console.Write("\n> ");
			string? option = Console.ReadLine();

			switch (option)
			{
				case "1":
					Console.WriteLine("Amount of words to put in new password:");
					settings.WordCount = ParseUserIntInput();
					break;

				case "2":
					Console.WriteLine("Amount of symbols to put in new password:");
					settings.SymbolCount = ParseUserIntInput();
					break;

				case "3":
					Console.WriteLine("Select symbol mode:\n[1] Insert symbols in random places\n[2] Replace spaces with symbols\n[3] Replace random characters with symbols");
					settings.SymbolMode = ParseUserIntInput();
					settings.SymbolMode = (settings.SymbolMode > 0 && settings.SymbolMode <= 3) ? settings.SymbolMode - 1 : 0;
					break;

				case "4":
					settings.RandomizeLetterSize = !settings.RandomizeLetterSize;
					break;

				case "5":
					settings.UseLetters = !settings.UseLetters;
					break;

				case "6":
					settings.UseCapitalLetters = !settings.UseCapitalLetters;
					break;

				case "7":
					settings.UseNumbers = !settings.UseNumbers;
					break;

				case "8":
					settings.UseSymbols = !settings.UseSymbols;
					break;

				case "9":
					settings.RandomizeParameters();
					break;

				case "0":
					DisplayMainMenu();
					break;

				default:
					break;
			}

			DisplayGeneratorMenu();
		}

		private static int ParseUserIntInput()
		{
			string? key = Console.ReadLine();
			int parsedValue;
			int.TryParse(key, out parsedValue);
			return parsedValue;
		}

		private static void DisplayPasswordEntropy(string password)
		{
			double entropy = Logic.Helpers.CalculateEntropy(password, ref _core.dataBank);

			SetEntropyColors(entropy);
			System.Console.WriteLine($"Password entropy: {entropy}");
			System.Console.ForegroundColor = ConsoleColor.White;
			Console.ReadKey();
		}

		private static void SetEntropyColors(double entropy)
		{

			if (entropy >= 100.0)
			{
				System.Console.ForegroundColor = ConsoleColor.Green;
				return;
			}

			if (entropy >= 65.0)
			{
				System.Console.ForegroundColor = ConsoleColor.Blue;
				return;
			}

			if (entropy > 0.0)
			{
				System.Console.ForegroundColor = ConsoleColor.Red;
			}
		}
	}
}