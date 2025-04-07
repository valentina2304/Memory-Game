using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.ViewModels;
using System;
using System.Windows;

namespace MemoryGame.Views
{
    public partial class GameView : Window
    {
        private GameViewModel _viewModel;

        public GameView()
        {
            InitializeComponent();
            _viewModel = new GameViewModel();
            this.DataContext = _viewModel;
        }

        public GameView(User currentUser)
        {
            InitializeComponent();
            _viewModel = new GameViewModel();
            this.DataContext = _viewModel;

            if (currentUser != null)
            {
                _viewModel.CurrentPlayer = currentUser;

                this.Title = $"Game Setup - {currentUser.Username}";
            }

        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

        }
    }
}