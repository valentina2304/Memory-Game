using System.Configuration;
using System.Data;
using System.Windows;
using MemoryGame.Views;

namespace MemoryGame
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Afisam fereastra de login ca fereastra principala
            var loginWindow = new LoginView();
            loginWindow.Show();
        }
    }
}
