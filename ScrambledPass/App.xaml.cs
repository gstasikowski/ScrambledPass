using ScrambledPass.Logic;
using ScrambledPass.Model;
using System.Windows;

namespace ScrambledPass
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DataBank dataBank = new DataBank();
        public FileOperations fileO = new FileOperations();
    }
}
