namespace ScrambledPass.ConsoleApp
{
	class Program
	{
		private static Core _core = new Core();
		private static GeneratorSettings settings = new GeneratorSettings();

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
				$"Word count: {settings.wordCount} ",
				$"⏐ Symbol count: {settings.symbolCount}",
				$"⏐ Symbol mode: {settings.symbolMode + 1}"
			);
			System.Console.WriteLine(
				"{0, -35} {1, -35} {2, -35}",
				$"Randomize letter size: {settings.randomizeLetterSize}",
				$"⏐ Use random letters: {settings.useLetters}",
				$"⏐ Use random capital letters: {settings.useCapitalLetters}"
			);
			System.Console.WriteLine(
				"{0, -35} {1, -35}",
				$"Use random numbers: {settings.useNumbers}",
				$"⏐ Use random symbols: {settings.useSymbols}\n"
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
					string newPassword = _core.generator.GeneratePassword(
						settings.wordCount,
						settings.symbolMode,
						settings.symbolCount,
						settings.randomizeLetterSize,
						settings.useLetters,
						settings.useCapitalLetters,
						settings.useNumbers,
						settings.useSymbols
					);

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
					ParseUserIntInput(ref settings.wordCount);
					break;

				case "2":
					Console.WriteLine("Amount of symbols to put in new password:");
					ParseUserIntInput(ref settings.symbolCount);
					break;

				case "3":
					Console.WriteLine("Select symbol mode:\n[1] Replace spaces with symbols\n[2] Replace random characters with symbols\n[3] Insert symbols in random places");
					ParseUserIntInput(ref settings.symbolMode);
					settings.symbolMode = (settings.symbolMode > 0 && settings.symbolMode <= 3) ? settings.symbolMode - 1 : 0;
					break;

				case "4":
					settings.randomizeLetterSize = !settings.randomizeLetterSize;
					break;

				case "5":
					settings.useLetters = !settings.useLetters;
					break;

				case "6":
					settings.useCapitalLetters = !settings.useCapitalLetters;
					break;

				case "7":
					settings.useNumbers = !settings.useNumbers;
					break;

				case "8":
					settings.useSymbols = !settings.useSymbols;
					break;

				case "9":
					settings.RandomizeSettings();
					break;

				case "0":
					DisplayMainMenu();
					break;

				default:
					break;
			}

			DisplayGeneratorMenu();
		}

		private static void ParseUserIntInput(ref int variable)
		{
			string? key = Console.ReadLine();
			int.TryParse(key, out variable);
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