using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.IO;
using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Views;
using System.ComponentModel;
using System.Text.Json;

namespace MemoryGame.ViewModels
{
    
    public class LoginViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private User _selectedUser;
        private string _newUsername;
        private string _selectedImagePath;
        private List<string> _availableAvatars;
        private int _currentAvatarIndex;

        public event EventHandler ShowNewUserDialogRequested;
        public event EventHandler CloseNewUserDialogRequested;
        public ObservableCollection<User> Users { get; private set; }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                OnPropertyChanged(nameof(IsUserSelected));
            }
        }

        public string NewUsername
        {
            get => _newUsername;
            set
            {
                _newUsername = value;
                OnPropertyChanged(nameof(NewUsername));
                OnPropertyChanged(nameof(CanCreateUser));
            }
        }

        public string SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged(nameof(SelectedImagePath));
                OnPropertyChanged(nameof(CanCreateUser));
            }
        }

        public bool IsUserSelected => SelectedUser != null;
        public bool CanCreateUser => !string.IsNullOrEmpty(NewUsername) && !string.IsNullOrEmpty(SelectedImagePath);

        public ICommand NewUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand BrowseImageCommand { get; }
        public ICommand CreateUserCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand NextAvatarCommand { get; }
        public ICommand PreviousAvatarCommand { get; }

        public LoginViewModel()
        {
            _userService = new UserService();
            LoadUsers();

            _availableAvatars = LoadAvailableAvatars();
            _currentAvatarIndex = 0;

            if (_availableAvatars.Count > 0)
                SelectedImagePath = _availableAvatars[0];

            NewUserCommand = new RelayCommand(_ => ShowNewUserDialog());
            DeleteUserCommand = new RelayCommand(_ => DeleteSelectedUser(), _ => IsUserSelected);
            PlayCommand = new RelayCommand(_ => StartGame(), _ => IsUserSelected);

            NextAvatarCommand = new RelayCommand(_ => SelectNextAvatar());
            PreviousAvatarCommand = new RelayCommand(_ => SelectPreviousAvatar());

            CreateUserCommand = new RelayCommand(_ => CreateNewUser(), _ => CanCreateUser);
            CancelCommand = new RelayCommand(_ => CloseNewUserDialog());
        }

        private void LoadUsers()
        {
            var usersList = _userService.LoadUsers();
            Users = new ObservableCollection<User>(usersList);
            OnPropertyChanged(nameof(Users));
        }

        private List<string> LoadAvailableAvatars()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string avatarsDir = Path.Combine(baseDir, "Resources", "Avatars");

            if (!Directory.Exists(avatarsDir))
            {
                Directory.CreateDirectory(avatarsDir);
            }

            string[] avatarFiles = Directory.GetFiles(avatarsDir, "*.jpg")
                                  .Concat(Directory.GetFiles(avatarsDir, "*.png"))
                                  .Concat(Directory.GetFiles(avatarsDir, "*.gif"))
                                  .ToArray();

            if (avatarFiles.Length == 0)
            {
                MessageBox.Show("Nu s-au găsit avatare. Te rugăm să adaugi imagini în directorul Avatars.",
                               "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
                return new List<string>();
            }

            return avatarFiles.ToList();
        }

        private void SelectNextAvatar()
        {
            if (_availableAvatars.Count == 0) return;

            _currentAvatarIndex = (_currentAvatarIndex + 1) % _availableAvatars.Count;
            SelectedImagePath = _availableAvatars[_currentAvatarIndex];
        }

        private void SelectPreviousAvatar()
        {
            if (_availableAvatars.Count == 0) return;

            _currentAvatarIndex = (_currentAvatarIndex - 1 + _availableAvatars.Count) % _availableAvatars.Count;
            SelectedImagePath = _availableAvatars[_currentAvatarIndex];
        }

        private void CreateNewUser()
        {
            try
            {
                if (string.IsNullOrEmpty(NewUsername) || string.IsNullOrEmpty(SelectedImagePath))
                {
                    MessageBox.Show("Trebuie să introduci un nume de utilizator și să selectezi o imagine.",
                                "Date incomplete", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newUser = new User
                {
                    Username = NewUsername,
                    ImagePath = SelectedImagePath,  
                    GamesPlayed = 0,
                    GamesWon = 0
                };

                _userService.AddUser(newUser);
                Users.Add(newUser);
                SelectedUser = newUser;

                CloseNewUserDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la crearea utilizatorului: {ex.Message}", "Eroare",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowNewUserDialog()
        {
            NewUsername = string.Empty;

            _currentAvatarIndex = 0;
            if (_availableAvatars.Count > 0)
                SelectedImagePath = _availableAvatars[_currentAvatarIndex];
            else
                SelectedImagePath = string.Empty;

            ShowNewUserDialogRequested?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteSelectedUser()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show($"Ești sigur că vrei să ștergi utilizatorul '{SelectedUser.Username}' și toate jocurile salvate?\nAceastă acțiune nu poate fi anulată!",
                                        "Confirmare ștergere",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DeleteUserSavedGames(SelectedUser.Username);

                    _userService.DeleteUser(SelectedUser.Username);
                    Users.Remove(SelectedUser);
                    SelectedUser = null;

                    MessageBox.Show($"Utilizatorul și toate jocurile salvate au fost șterse cu succes.",
                                   "Ștergere reușită",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"A apărut o eroare la ștergerea utilizatorului: {ex.Message}",
                                   "Eroare",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Error);
                }
            }
        }

        private void DeleteUserSavedGames(string username)
        {
            try
            {
                string saveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SavedGames");

                if (Directory.Exists(saveDir))
                {
                    var savedFiles = Directory.GetFiles(saveDir, "*.mem");

                    foreach (var file in savedFiles)
                    {
                        try
                        {
                            string json = File.ReadAllText(file);
                            var gameState = JsonSerializer.Deserialize<GameState>(json);

                            if (gameState != null && gameState.PlayerName == username)
                            {
                                File.Delete(file);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Eroare la ștergerea jocurilor salvate: {ex.Message}", ex);
            }
        }

        private void StartGame()
        {
            if (SelectedUser == null) return;

            try
            {
                var gameView = new GameView(SelectedUser);

                gameView.Show();

                if (Application.Current.MainWindow is Window currentWindow)
                {
                    Application.Current.MainWindow = gameView;
                    currentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la pornirea jocului: {ex.Message}", "Eroare",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseNewUserDialog()
        {
            NewUsername = string.Empty;
            SelectedImagePath = string.Empty;
            CloseNewUserDialogRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}