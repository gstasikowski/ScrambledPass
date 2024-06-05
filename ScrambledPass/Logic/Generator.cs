using System.Text;
using ScrambledPass.Helpers;
using ScrambledPass.Models;

namespace ScrambledPass.Logic
{
	public class Generator
	{
		private DataBank _dataBank;

		private Random _randomInt = new Random();
		private List<char> _availableCharacters = new List<char>();

		public Generator(DataBank dataBank)
		{
			_dataBank = dataBank;
		}

		#region Methods (public)
		public string GeneratePassword(
			int wordCount,
			int symbolMode,
			int symbolCount,
			bool randomCharacterSize,
			bool useLetters,
			bool useCapitalLetters,
			bool useNumbers,
			bool useSymbols
		)
		{
			StringBuilder passwordBuilder = new StringBuilder();

			PrepareCharacterList(
				wordCount,
				useLetters,
				useCapitalLetters,
				useNumbers,
				useSymbols
			);

			InsertWords(wordCount, ref passwordBuilder);

			if (randomCharacterSize)
			{
				RandomizeLetterSize(ref passwordBuilder);
			}

			InsertSymbols(
				symbolMode,
				symbolCount,
				useLetters,
				useCapitalLetters,
				useNumbers,
				useSymbols,
				ref passwordBuilder
			);

			return passwordBuilder.ToString().Trim();
		}
		#endregion Methods (public)

		#region Methods (private)
		private void PrepareCharacterList(
			int wordCount,
			bool useLetters,
			bool useCapitalLetters,
			bool useNumbers,
			bool useSymbols
		)
		{
			_availableCharacters.Clear();

			if (useLetters)
			{
				for (int i = 0; i < 26; i++)
				{
					_availableCharacters.Add((char)('a' + i));
				}
			}

			if (useCapitalLetters)
			{
				for (int i = 0; i < 26; i++)
				{
					_availableCharacters.Add((char)('A' + i));
				}
			}

			if (useNumbers)
			{
				for (int i = 48; i < 58; i++)
				{
					_availableCharacters.Add((char)i);
				}
			}

			if (useSymbols)
			{
				_availableCharacters.AddRange(_dataBank.Symbols);
			}

			if (wordCount > 0 && !useSymbols)
			{
				_availableCharacters.Add(' ');
			}
		}

		private void InsertWords(int wordCount, ref StringBuilder passwordBuilder)
		{
			if (_dataBank.WordList.Count < 1)
			{
				new ErrorHandler("WordListEmpty");
			}

			for (int i = 0; i < wordCount; i++)
			{
				passwordBuilder.Append(_dataBank.WordList[_randomInt.Next(0, _dataBank.WordList.Count)] + " ");
			}
		}

		private void InsertSymbols(
			int symbolMode,
			int symbolCount,
			bool useLetters,
			bool useCapitalLetters,
			bool useNumbers,
			bool useSymbols,
			ref StringBuilder passwordBuilder
		)
		{
			if (useLetters || useCapitalLetters || useNumbers || useSymbols)
			{
				if (passwordBuilder.Length == 0)
				{
					symbolMode = (int)SymbolMode.RandomInsert;
				}

				switch (symbolMode)
				{
					case (int)SymbolMode.RandomSpacing:
						RandomizeSpacing(ref passwordBuilder, symbolCount);
						break;

					case (int)SymbolMode.ReplaceCharacters:
						ReplaceRandomCharacters(ref passwordBuilder, symbolCount);
						break;

					case (int)SymbolMode.RandomInsert:
						InsertRandomCharacters(ref passwordBuilder, symbolCount);
						break;

					default:
						break;
				}
			}
		}

		private char GetRandomSymbol()
		{
			if (_availableCharacters.Count < 1)
			{
				return ' ';
			}

			return _availableCharacters[_randomInt.Next(0, _availableCharacters.Count)];
		}

		private void RandomizeSpacing(ref StringBuilder passwordBuilder, int characterCount)
		{
			StringBuilder spacingBuilder = new StringBuilder();

			for (int index = 0; index < passwordBuilder.Length; index++)
			{
				spacingBuilder.Clear();

				if (passwordBuilder[index] == ' ')
				{
					int spacingWidth = _randomInt.Next(0, characterCount);

					for (int spaceIndex = 0; spaceIndex < spacingWidth; spaceIndex++)
					{
						spacingBuilder.Append(GetRandomSymbol());
					}

					passwordBuilder.Remove(index, 1);
					passwordBuilder.Insert(index, spacingBuilder);
				}
			}
		}

		private void RandomizeLetterSize(ref StringBuilder passwordBuilder)
		{
			int charactersToChange = _randomInt.Next(0, passwordBuilder.Length);

			for (int changedCount = 0; changedCount < charactersToChange; changedCount++)
			{
				int characterIndex = _randomInt.Next(0, passwordBuilder.Length);
				passwordBuilder[characterIndex] = char.ToUpper(passwordBuilder[characterIndex]);
			}
		}

		private void ReplaceRandomCharacters(ref StringBuilder passwordBuilder, int characterCount)
		{
			for (int i = 0; i < characterCount; i++)
			{
				passwordBuilder[_randomInt.Next(0, passwordBuilder.Length)] = GetRandomSymbol();
			}
		}

		private void InsertRandomCharacters(ref StringBuilder passwordBuilder, int characterCount)
		{
			for (int i = 0; i < characterCount; i++)
			{
				int charPosition = (passwordBuilder.Length > 0) ? _randomInt.Next(0, passwordBuilder.Length) : 0;

				passwordBuilder.Insert(charPosition, GetRandomSymbol());
			}
		}
		#endregion Methods (private)
	}
}
