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
    /// <summary>
    /// ViewModel pentru fereastra de autentificare
    /// </summary>
    public class LoginViewModel : INotifyPropertyChanged
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

            // Inițializăm lista de avatare disponibile
            _availableAvatars = LoadAvailableAvatars();
            _currentAvatarIndex = 0;

            // Setăm un avatar inițial pentru noi utilizatori
            if (_availableAvatars.Count > 0)
                SelectedImagePath = _availableAvatars[0];

            // Comenzile existente...
            NewUserCommand = new RelayCommand(_ => ShowNewUserDialog());
            DeleteUserCommand = new RelayCommand(_ => DeleteSelectedUser(), _ => IsUserSelected);
            PlayCommand = new RelayCommand(_ => StartGame(), _ => IsUserSelected);

            // Adăugăm comenzi pentru navigarea între avatare
            NextAvatarCommand = new RelayCommand(_ => SelectNextAvatar());
            PreviousAvatarCommand = new RelayCommand(_ => SelectPreviousAvatar());

            // Restul comenzilor...
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
            // Folosim calea relativă la directorul aplicației
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string avatarsDir = Path.Combine(baseDir, "Resources", "Avatars");

            // Dacă directorul nu există, îl creăm
            if (!Directory.Exists(avatarsDir))
            {
                Directory.CreateDirectory(avatarsDir);
            }

            // Căutăm toate fișierele de imagini din director
            string[] avatarFiles = Directory.GetFiles(avatarsDir, "*.jpg")
                                  .Concat(Directory.GetFiles(avatarsDir, "*.png"))
                                  .Concat(Directory.GetFiles(avatarsDir, "*.gif"))
                                  .ToArray();

            // Dacă nu găsim imagini, returnăm o listă goală sau implicit
            if (avatarFiles.Length == 0)
            {
                // Creăm niște avatare default în memoria aplicației
                // Această soluție este temporară - ar fi mai bine să incluzi
                // câteva avatare implicite în proiect
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
                // Asigurăm-ne că avem un username și o cale către imagine
                if (string.IsNullOrEmpty(NewUsername) || string.IsNullOrEmpty(SelectedImagePath))
                {
                    MessageBox.Show("Trebuie să introduci un nume de utilizator și să selectezi o imagine.",
                                "Date incomplete", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newUser = new User
                {
                    Username = NewUsername,
                    ImagePath = SelectedImagePath,  // Păstrăm calea selectată - UserService va converti la relativă la salvare
                    GamesPlayed = 0,
                    GamesWon = 0
                };

                _userService.AddUser(newUser);
                Users.Add(newUser);
                SelectedUser = newUser;

                // Închide dialogul
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

            // Resetăm avatarul la primul din listă când deschidem dialogul
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
                    // 1. Ștergem jocurile salvate ale utilizatorului
                    DeleteUserSavedGames(SelectedUser.Username);

                    // 2. Ștergem utilizatorul din lista
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
                    // Obținem toate fișierele salvate
                    var savedFiles = Directory.GetFiles(saveDir, "*.mem");

                    foreach (var file in savedFiles)
                    {
                        try
                        {
                            // Citim conținutul fișierului
                            string json = File.ReadAllText(file);
                            var gameState = JsonSerializer.Deserialize<GameState>(json);

                            // Verificăm dacă jocul aparține utilizatorului care va fi șters
                            if (gameState != null && gameState.PlayerName == username)
                            {
                                // Ștergem fișierul
                                File.Delete(file);
                            }
                        }
                        catch
                        {
                            // Ignorăm erorile individuale la citirea fișierelor
                            // și continuăm cu următorul fișier
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Propagăm eroarea pentru a fi tratată în metoda apelantă
                throw new Exception($"Eroare la ștergerea jocurilor salvate: {ex.Message}", ex);
            }
        }

        private void StartGame()
        {
            if (SelectedUser == null) return;

            try
            {
                // Creăm o nouă instanță a ferestrei de joc și îi pasăm utilizatorul curent
                var gameView = new GameView(SelectedUser);

                // Afișăm fereastra jocului
                gameView.Show();

                // Închide fereastra de login
                if (Application.Current.MainWindow is Window currentWindow)
                {
                    // Setăm noua fereastră ca MainWindow
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseNewUserDialog()
        {
            NewUsername = string.Empty;
            SelectedImagePath = string.Empty;
            CloseNewUserDialogRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}