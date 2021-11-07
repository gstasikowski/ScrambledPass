namespace ScrambledPass.Logic
{
    class Refs
    {
        public static Model.DataBank dataBank = new Model.DataBank();
        public static FileOperations fileOperations = new FileOperations();
        public static LocalizationHandler localizationHandler = new LocalizationHandler();
        public static Generator passGen = new Generator();
    }
}
