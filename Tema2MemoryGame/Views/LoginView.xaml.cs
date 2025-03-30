using System.Windows;

namespace Tema2MemoryGame.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new ViewModels.LoginViewModel();
        }
    }
}