using ScrambledPass.Logic;
using ScrambledPass.Models;

namespace ScrambledPass
{
	public class Core
	{
		public DataBank dataBank;
		public Generator generator;
		public FileOperations fileOperations;

		public static Core Instance { get; set; } = new Core();

		public Core()
		{
			dataBank = new DataBank();
			generator = new Generator(dataBank);
			fileOperations = new FileOperations(dataBank);
		}
	}
}
