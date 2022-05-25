using ScrambledPass.Models;

namespace ScrambledPass.Logic
{
    public static class Refs
    {
        public static ApplicationViewModel viewControl;
        public static DataBank dataBank = new DataBank();
        public static Generator generator = new Generator();
        public static ResourceHandler resourceHandler = new ResourceHandler();
    }
}
