using System.Configuration;
using System.Data;
using System.Windows;
using Tema2MemoryGame.Views;
using Tema2MemoryGame.ViewModels;

namespace Tema2MemoryGame;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = new LoginView { DataContext = new LoginViewModel() };
        mainWindow.Show();
    }
}
