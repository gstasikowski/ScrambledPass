using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScrambledPass.Models
{
	public class GeneratorParameters : INotifyPropertyChanged
	{
		private int _wordCount;
		private int _symbolMode;
		private int _symbolCount;
		private bool _randomizeLetterSize;
		private bool _useLetters;
		private bool _useCapitalLetters;
		private bool _useNumbers;
		private bool _useSymbols;

		public event PropertyChangedEventHandler PropertyChanged;

		public int WordCount
		{
			get { return _wordCount; }
			set
			{
				_wordCount = value;
				OnPropertyChanged();
			}
		}
		public int SymbolMode
		{
			get { return _symbolMode; }
			set
			{
				_symbolMode = value;
				OnPropertyChanged();
			}
		}
		public int SymbolCount
		{
			get { return _symbolCount; }
			set
			{
				_symbolCount = value;
				OnPropertyChanged();
			}
		}
		public bool RandomizeLetterSize
		{
			get { return _randomizeLetterSize; }
			set
			{
				_randomizeLetterSize = value;
				OnPropertyChanged();
			}
		}
		public bool UseLetters
		{
			get { return _useLetters; }
			set
			{
				_useLetters = value;
				OnPropertyChanged();
			}
		}
		public bool UseCapitalLetters
		{
			get { return _useCapitalLetters; }
			set
			{
				_useCapitalLetters = value;
				OnPropertyChanged();
			}
		}
		public bool UseNumbers
		{
			get { return _useNumbers; }
			set
			{
				_useNumbers = value;
				OnPropertyChanged();
			}
		}
		public bool UseSymbols
		{
			get { return _useSymbols; }
			set
			{
				_useSymbols = value;
				OnPropertyChanged();
			}
		}

		public GeneratorParameters()
		{ }

		public GeneratorParameters(int wordCount, int symbolMode, int symbolCount, bool randomizeLetterSize, bool useLetters, bool useCapitalLetters, bool useNumbers, bool useSymbols)
		{
			WordCount = wordCount;
			SymbolMode = symbolMode;
			SymbolCount = symbolCount;
			RandomizeLetterSize = randomizeLetterSize;
			UseLetters = useLetters;
			UseCapitalLetters = useCapitalLetters;
			UseNumbers = useNumbers;
			UseSymbols = useSymbols;
		}

		public void RandomizeParameters()
		{
			Random randomizer = new Random();

			WordCount = randomizer.Next(10);
			SymbolMode = randomizer.Next(3);
			SymbolCount = randomizer.Next(20);
			RandomizeLetterSize = randomizer.Next(2) > 0;
			UseLetters = randomizer.Next(2) > 0;
			UseCapitalLetters = randomizer.Next(2) > 0;
			UseNumbers = randomizer.Next(2) > 0;
			UseSymbols = randomizer.Next(2) > 0;
		}

		// Create the OnPropertyChanged method to raise the event
		// The calling member's name will be used as the parameter.
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}