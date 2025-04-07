using MemoryGame.ViewModels;
using System;
using System.Windows;

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView : Window
    {
        private GameBoardViewModel _viewModel;

        public GameBoardView(GameBoardViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = _viewModel;
        }
        /// <summary>
        /// Oprește timer-ul jocului în mod explicit
        /// </summary>


        // Dacă se închide fereastra direct, ne asigurăm că oprim timer-ul
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Oprim explicit timer-ul din ViewModel înainte de a închide fereastra
            if (_viewModel != null)
            {
                _viewModel.StopTimer();
            }

            // Când fereastra este închisă, verificăm dacă ea este încă MainWindow
            if (Application.Current.MainWindow == this)
            {
                var loginView = new LoginView();
                loginView.Show();
                Application.Current.MainWindow = loginView;
            }
        }
    }
}