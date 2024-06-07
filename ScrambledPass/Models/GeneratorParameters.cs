namespace ScrambledPass.Models
{
	public class GeneratorParameters
	{
		public int WordCount;
		public int SymbolMode;
		public int SymbolCount;
		public bool RandomizeLetterSize;
		public bool UseLetters;
		public bool UseCapitalLetters;
		public bool UseNumbers;
		public bool UseSymbols;

		public void RandomizeParameters()
		{
			Random randomizer = new Random();

			WordCount = randomizer.Next(10);
			SymbolMode = randomizer.Next(3);
			SymbolCount = randomizer.Next(20);
			RandomizeLetterSize = randomizer.Next(1) > 0;
			UseLetters = randomizer.Next(1) > 0;
			UseCapitalLetters = randomizer.Next(1) > 0;
			UseNumbers = randomizer.Next(1) > 0;
			UseSymbols = randomizer.Next(1) > 0;
		}
	}
}