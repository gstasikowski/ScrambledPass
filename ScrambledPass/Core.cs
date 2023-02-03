using ScrambledPass.Logic;
using ScrambledPass.Models;

namespace ScrambledPass
{
    public class Core
    {
        public DataBank dataBank;
        public Generator generator;
        public FileOperations fileOperations;

        public Core()
        {
            dataBank = new DataBank();
            generator = new Generator(dataBank);
            fileOperations = new FileOperations(dataBank);
        }
    }
}
