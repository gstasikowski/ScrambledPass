using ScrambledPass.Models;

namespace ScrambledPass.Logic
{
    public static class Refs
    {
        public static ApplicationViewModel viewControl;
        public static DataBank dataBank = new DataBank();
        public static FileOperations fileOperations = new FileOperations();
        public static ResourceHandler resourceHandler = new ResourceHandler();
        public static Generator passGen = new Generator();
    }
}
