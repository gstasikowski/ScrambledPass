namespace ScrambledPass.ConsoleApp
{
	public class GeneratorSettings
	{
		public int wordCount = 0;
		public int symbolMode = 0;
		public int symbolCount = 10;
		public bool randomizeLetterSize = false;
		public bool useLetters = true;
		public bool useCapitalLetters = true;
		public bool useNumbers = true;
		public bool useSymbols = true;

		public void RandomizeSettings()
		{
			Random randomizer = new Random();

			wordCount = randomizer.Next(10);
			symbolMode = randomizer.Next(3);
			symbolCount = randomizer.Next(20);
			randomizeLetterSize = randomizer.Next(1) > 0;
			useLetters = randomizer.Next(1) > 0;
			useCapitalLetters = randomizer.Next(1) > 0;
			useNumbers = randomizer.Next(1) > 0;
			useSymbols = randomizer.Next(1) > 0;
		}
	}
}