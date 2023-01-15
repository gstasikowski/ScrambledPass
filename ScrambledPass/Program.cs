﻿namespace ScrambledPass.Logic
{
    class Program
    {
        public static void Main(string[] args)
        {
            Logic.FileOperations.LoadResources();
            ShowMenu();
		}

        private static void ShowMenu()
        {
			int wordCount = 0;
			int symbolMode = 0;
			int symbolCount = 10;
			bool randomizeLetterSize = false;
			bool useLetters = true;
			bool useBigLetters = true;
			bool useNumbers = true;
			bool useSymbols = true;

			MenuStart:
			Console.Clear();
			Console.WriteLine("Current settings\n----------------");
			Console.WriteLine(
				"Word count: {0} | Symbol mode: {1} | Symbol count: {2}\nRandomize letter size: {3} | Use random letters: {4} | Use random capital letters: {5}\nUse random numbers: {6} | Use random symbols: {7}",
				wordCount,
                	symbolMode,
                    symbolCount,
                    randomizeLetterSize,
                    useLetters,
                    useBigLetters,
                    useNumbers,
                    useSymbols
			);
			Console.WriteLine("What do you want to do?\n1 - Generate password\n2 - Set word count\n3 - Toggle letter size randomization\n4 - Set symbol count\n5 - Set symbol mode\n6 - Toggle random letters\n7 - Toggle big letters\n8 - Toggle numbers\n9 - Toggle symbols\n10 - Randomize settings\n11 - Exit");
			Console.Write("\n> ");
			string? option = Console.ReadLine();

			switch (option)
			{
				case "1":
					Console.WriteLine("\nHere's your new password:");
                    string newPassword = Refs.generator.GeneratePassword(
                        wordCount,
                        symbolMode,
                        symbolCount,
                        randomizeLetterSize,
                        useLetters,
                        useBigLetters,
                        useNumbers,
                        useSymbols
                    );

                    Console.WriteLine(newPassword);
					Console.ReadKey();
					break;

				case "2":
					Console.WriteLine("Amount of words to put in new password:");
					ParseUserIntInput(ref wordCount);
					break;

				case "3":
					Console.WriteLine("Randomize letter size [Y\\N]:");
					randomizeLetterSize = ParseUserBoolInput();
					break;

				case "4":
					Console.WriteLine("Amount of symbols to put in new password:");
					ParseUserIntInput(ref symbolCount);
					break;

				case "5":
					Console.WriteLine("Select symbol mode:\n1 - Replace spaces with symbols\n2 - Replace random characters with symbols\n3 - Insert symbols in random places");
					ParseUserIntInput(ref symbolMode);
					break;

				case "6":
					Console.WriteLine("Insert random letters [Y\\N]:");
					useLetters = ParseUserBoolInput();
					break;

				case "7":
					Console.WriteLine("Insert random big letters [Y\\N]:");
					useBigLetters = ParseUserBoolInput();
					break;

				case "8":
					Console.WriteLine("Insert random numbers [Y\\N]:");
					useNumbers = ParseUserBoolInput();
					break;

				case "9":
					Console.WriteLine("Insert random symbols [Y\\N]:");
					useSymbols = ParseUserBoolInput();
					break;

				case "10":
					Random randomizer = new Random();
					wordCount = randomizer.Next(10);
                    symbolMode = randomizer.Next(3);
                    symbolCount = randomizer.Next(20);
                    randomizeLetterSize = randomizer.Next(1) > 0;
                    useLetters = randomizer.Next(1) > 0;
                    useBigLetters = randomizer.Next(1) > 0;
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

		private static bool ParseUserBoolInput()
		{
			string? key = Console.ReadLine();
			return (key == "Y") ? true : false;
		}
    }
}