using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Tema2MemoryGame.Commands;
using Tema2MemoryGame.Models;
using Tema2MemoryGame.Services;

namespace Tema2MemoryGame.ViewModels
{
    public partial class LoginViewModel : INotifyPropertyChanged
    {
        private readonly string _avatarsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "avatars");
        private readonly string[] _availableAvatars;
        private int _currentAvatarIndex;
        private User? _selectedUser;
        private string? _newUsername;

        public ObservableCollection<User> Users { get; } = UserService.LoadUsers();
        public bool CanPlayOrDelete => SelectedUser != null;

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanPlayOrDelete));
                }
            }
        }

        public string? NewUsername
        {
            get => _newUsername;
            set
            {
                if (_newUsername != value)
                {
                    _newUsername = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand PreviousAvatarCommand { get; }
        public ICommand NextAvatarCommand { get; }

        public LoginViewModel()
        {
            Directory.CreateDirectory(_avatarsFolder);
            _availableAvatars = Directory.GetFiles(_avatarsFolder, "*.png");

            AddUserCommand = new RelayCommand(AddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, _ => CanPlayOrDelete);
            PlayCommand = new RelayCommand(PlayGame, _ => CanPlayOrDelete);
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
            PreviousAvatarCommand = new RelayCommand(_ => CycleAvatar(-1));
            NextAvatarCommand = new RelayCommand(_ => CycleAvatar(1));
        }

        private void AddUser(object? _)
        {
            if (string.IsNullOrWhiteSpace(NewUsername))
            {
                MessageBox.Show("Please enter a username", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new User(NewUsername!.Trim(), _availableAvatars[_currentAvatarIndex]);
            Users.Add(newUser);
            SelectedUser = newUser; // Automatically select the new user
            UserService.SaveUsers(Users);
            NewUsername = string.Empty;
        }

        private void DeleteUser(object? _)
        {
            if (SelectedUser != null && MessageBox.Show(
                $"Delete user {SelectedUser.Username}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Users.Remove(SelectedUser);
                UserService.SaveUsers(Users);
            }
        }

        private void PlayGame(object? _)
        {
            if (SelectedUser != null)
            {
                MessageBox.Show($"Starting game with {SelectedUser.Username}", "Game Starting");
            }
        }

        private void CycleAvatar(int direction)
        {
            _currentAvatarIndex = (_currentAvatarIndex + direction + _availableAvatars.Length) % _availableAvatars.Length;
            if (SelectedUser != null)
            {
                SelectedUser.AvatarPath = _availableAvatars[_currentAvatarIndex];
                UserService.SaveUsers(Users);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}