namespace ScrambledPass.ConsoleApp
{
    class Program
    {
		private static Core _core = new Core();

        public static void Main(string[] args)
        {
            _core.fileOperations.LoadResources();
            ShowMenu();
		}

        private static void ShowMenu()
        {
			int wordCount = 0;
			int symbolMode = 0;
			int symbolCount = 10;
			bool randomizeLetterSize = false;
			bool useLetters = true;
			bool useCapitalLetters = true;
			bool useNumbers = true;
			bool useSymbols = true;

			MenuStart:
			Console.Clear();
			Console.WriteLine("Current settings\n――――――――――――――――");
			Console.WriteLine(
				"{0, -35} {1, -35} {2, -35}",
				$"Word count: {wordCount} ",
				$"⏐ Symbol mode: {symbolMode + 1}",
				$"⏐ Symbol count: {symbolCount}"
			);
			System.Console.WriteLine(
				"{0, -35} {1, -35} {2, -35}",
				$"Randomize letter size: {randomizeLetterSize}",
				$"⏐ Use random letters: {useLetters}",
				$"⏐ Use random capital letters: {useCapitalLetters}"
			);
			System.Console.WriteLine(
				"{0, -35} {1, -35}",
				$"Use random numbers: {useNumbers}",
				$"⏐ Use random symbols: {useSymbols}\n"
			);
			Console.WriteLine("What do you want to do?\n1 - Generate password\n2 - Set word count\n3 - Set symbol count\n4 - Set symbol mode\n5 - Toggle letter size randomization\n6 - Toggle random letters\n7 - Toggle captial letters\n8 - Toggle numbers\n9 - Toggle symbols\n10 - Randomize settings\n11 - Exit");
			Console.Write("\n> ");
			string? option = Console.ReadLine();

			switch (option)
			{
				case "1":
					Console.WriteLine("\nHere's your new password:");
                    string newPassword = _core.generator.GeneratePassword(
                        wordCount,
                        symbolMode,
                        symbolCount,
                        randomizeLetterSize,
                        useLetters,
                        useCapitalLetters,
                        useNumbers,
                        useSymbols
                    );

                    Console.WriteLine(newPassword);
					DisplayPasswordEntropy(newPassword);
					Console.ReadKey();
					break;

				case "2":
					Console.WriteLine("Amount of words to put in new password:");
					ParseUserIntInput(ref wordCount);
					break;

				case "3":
					Console.WriteLine("Amount of symbols to put in new password:");
					ParseUserIntInput(ref symbolCount);
					break;

				case "4":
					Console.WriteLine("Select symbol mode:\n1 - Replace spaces with symbols\n2 - Replace random characters with symbols\n3 - Insert symbols in random places");
					ParseUserIntInput(ref symbolMode);
					symbolMode = (symbolMode > 0 && symbolMode <= 3) ? symbolMode - 1 : 0;
					break;

				case "5":
					randomizeLetterSize = !randomizeLetterSize;
					break;

				case "6":
					useLetters = !useLetters;
					break;

				case "7":
					useCapitalLetters = !useCapitalLetters;
					break;

				case "8":
					useNumbers = !useNumbers;
					break;

				case "9":
					useSymbols = !useSymbols;
					break;

				case "10":
					Random randomizer = new Random();
					wordCount = randomizer.Next(10);
                    symbolMode = randomizer.Next(3);
                    symbolCount = randomizer.Next(20);
                    randomizeLetterSize = randomizer.Next(1) > 0;
                    useLetters = randomizer.Next(1) > 0;
                    useCapitalLetters = randomizer.Next(1) > 0;
                    useNumbers = randomizer.Next(1) > 0;
                    useSymbols = randomizer.Next(1) > 0;
					break;

				case "11":
					System.Environment.Exit(1);
					break;

				default:
					break;
			}

			goto MenuStart;
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