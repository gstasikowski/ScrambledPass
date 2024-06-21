namespace ScrambledPass.UnitTests
{
	public class PasswordGenerationTests
	{
		private const int SymbolMode = 2;
		private const int SymbolCount = 10;
		private static char[] _skippedCharacters = { ' ', '.', '&', '\'' };

		private Core _core;

		public PasswordGenerationTests()
		{
			_core = new Core();
			_core.dataBank.SetSetting(key: "lastWordList", value: string.Empty);
			_core.fileOperations.PrepareWordList();
		}

		[Theory]
		[InlineData(2, 0, 0, false, false, false, false, false)]
		[InlineData(5, 0, 0, false, false, false, false, false)]
		[InlineData(10, 0, 0, false, false, false, false, false)]
		public void Should_generate_word_only_password(
			int wordCount,
			int symbolMode,
			int symbolCount,
			bool randomizeLetterSize,
			bool useLetters,
			bool useCapitalLetters,
			bool useNumbers,
			bool useSymbols
		)
		{
			ScrambledPass.Models.GeneratorParameters parameters = new ScrambledPass.Models.GeneratorParameters(
				wordCount,
						symbolMode,
						symbolCount,
						randomizeLetterSize,
						useLetters,
						useCapitalLetters,
						useNumbers,
						useSymbols
						);

			string newPassword = _core.generator.GeneratePassword(parameters);

			Assert.True(
				newPassword.Split(' ').Count() == wordCount
				&& !PasswordContainsNumbers(newPassword)
				&& !PasswordContainsSymbols(newPassword)
			);
		}

		[Fact]
		public void Should_generate_letter_only_password()
		{
			ScrambledPass.Models.GeneratorParameters parameters = new ScrambledPass.Models.GeneratorParameters(
						wordCount: 0,
						symbolMode: SymbolMode,
						symbolCount: SymbolCount,
						randomizeLetterSize: false,
						useLetters: true,
						useCapitalLetters: false,
						useNumbers: false,
						useSymbols: false
					);

			string newPassword = _core.generator.GeneratePassword(parameters);

			Assert.True(
				PasswordContainsLowercaseLetters(newPassword)
				&& !PasswordContainsUppercaseLetters(newPassword)
				&& !PasswordContainsNumbers(newPassword)
				&& !PasswordContainsSymbols(newPassword)
			);
		}

		[Fact]
		public void Should_generate_capital_letter_only_password()
		{
			ScrambledPass.Models.GeneratorParameters parameters = new ScrambledPass.Models.GeneratorParameters(
						wordCount: 0,
						symbolMode: SymbolMode,
						symbolCount: SymbolCount,
						randomizeLetterSize: false,
						useLetters: false,
						useCapitalLetters: true,
						useNumbers: false,
						useSymbols: false
					);

			string newPassword = _core.generator.GeneratePassword(parameters);

			Assert.True(
				!PasswordContainsLowercaseLetters(newPassword)
				&& PasswordContainsUppercaseLetters(newPassword)
				&& !PasswordContainsNumbers(newPassword)
				&& !PasswordContainsSymbols(newPassword)
			);
		}

		[Fact]
		public void Should_generate_number_only_password()
		{
			ScrambledPass.Models.GeneratorParameters parameters = new ScrambledPass.Models.GeneratorParameters(
						wordCount: 0,
						symbolMode: SymbolMode,
						symbolCount: SymbolCount,
						randomizeLetterSize: false,
						useLetters: false,
						useCapitalLetters: false,
						useNumbers: true,
						useSymbols: false
					);

			string newPassword = _core.generator.GeneratePassword(parameters);

			Assert.True(
				!PasswordContainsLowercaseLetters(newPassword)
				&& !PasswordContainsUppercaseLetters(newPassword)
				&& PasswordContainsNumbers(newPassword)
				&& !PasswordContainsSymbols(newPassword)
			);
		}

		[Fact]
		public void Should_generate_symbol_only_password()
		{
			ScrambledPass.Models.GeneratorParameters parameters = new ScrambledPass.Models.GeneratorParameters(
						wordCount: 0,
						symbolMode: SymbolMode,
						symbolCount: SymbolCount,
						randomizeLetterSize: false,
						useLetters: false,
						useCapitalLetters: false,
						useNumbers: false,
						useSymbols: true
					);

			string newPassword = _core.generator.GeneratePassword(parameters);

			Assert.True(
				!PasswordContainsLowercaseLetters(newPassword)
				&& !PasswordContainsUppercaseLetters(newPassword)
				&& !PasswordContainsNumbers(newPassword)
				&& PasswordContainsSymbols(newPassword)
			);
		}

		private bool PasswordContainsLowercaseLetters(string password)
		{
			return password.Any(char.IsLower);
		}

		private bool PasswordContainsUppercaseLetters(string password)
		{
			return password.Any(char.IsUpper);
		}

		private bool PasswordContainsNumbers(string password)
		{
			return password.Any(char.IsNumber);
		}

		private bool PasswordContainsSymbols(string password)
		{
			foreach (char symbol in _core.dataBank.Symbols)
			{
				if (_skippedCharacters.Any(x => x == symbol))
				{
					continue;
				}

				if (password.Any(x => x == symbol))
				{
					return true;
				}
			}

			return false;
		}
	}
}