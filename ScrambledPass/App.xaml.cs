using ScrambledPass.Models;
using System.Windows;

namespace ScrambledPass
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool appReady = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Logic.FileOperations.LoadResources();

            MainWindow app = new MainWindow();
            ApplicationViewModel context = new ApplicationViewModel();
            Logic.Refs.viewControl = context;
            app.DataContext = context;
            app.Show();
        }
    }
}
