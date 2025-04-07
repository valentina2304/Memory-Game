using MemoryGame.ViewModels;
using System;
using System.Windows;

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();

            // Obținem view model-ul din resurse
            _viewModel = (LoginViewModel)Resources["LoginViewModel"];

            // Înregistrăm evenimentele pentru a gestiona dialogul de creare utilizator
            _viewModel.ShowNewUserDialogRequested += (s, e) => ShowNewUserDialog();
            _viewModel.CloseNewUserDialogRequested += (s, e) => HideNewUserDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Afișează dialogul de creare utilizator
        /// </summary>
        public void ShowNewUserDialog()
        {
            NewUserDialog.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Ascunde dialogul de creare utilizator
        /// </summary>
        public void HideNewUserDialog()
        {
            NewUserDialog.Visibility = Visibility.Collapsed;
        }
    }
}