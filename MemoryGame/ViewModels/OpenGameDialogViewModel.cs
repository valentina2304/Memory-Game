using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using MemoryGame.Commands;
using MemoryGame.Models;

namespace MemoryGame.ViewModels
{
    public class OpenGameDialogViewModel : ViewModelBase
    {
        private GameState _selectedGame;
        private User _currentPlayer;
        public ObservableCollection<GameState> SavedGames { get; } = new ObservableCollection<GameState>();

        public GameState SelectedGame
        {
            get => _selectedGame;
            set
            {
                if (_selectedGame != value)
                {
                    _selectedGame = value;
                    OnPropertyChanged();
                    DeleteGameCommand.RaiseCanExecuteChanged();
                    OpenGameCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand DeleteGameCommand { get; }
        public RelayCommand OpenGameCommand { get; }
        public RelayCommand CancelCommand { get; }

        public OpenGameDialogViewModel(User currentPlayer)
        {
            _currentPlayer = currentPlayer ?? throw new ArgumentNullException(nameof(currentPlayer));

            DeleteGameCommand = new RelayCommand(DeleteGame, CanExecuteGameCommand);
            OpenGameCommand = new RelayCommand(OpenGame, CanExecuteGameCommand);
            CancelCommand = new RelayCommand(Cancel);

            LoadSavedGames();
        }

        private bool CanExecuteGameCommand(object parameter)
        {
            return SelectedGame != null;
        }

        private void LoadSavedGames()
        {
            SavedGames.Clear();
            string saveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SavedGames");

            if (Directory.Exists(saveDir))
            {
                var savedFiles = Directory.GetFiles(saveDir, "*.mem");

                foreach (var file in savedFiles)
                {
                    try
                    {
                        if (!File.Exists(file))
                            continue;

                        string json = File.ReadAllText(file);
                        var gameState = JsonSerializer.Deserialize<GameState>(json);

                        if (gameState != null && !gameState.IsCompleted)
                        {
                            // Verificăm dacă jocul aparține utilizatorului curent
                            if (gameState.PlayerName == _currentPlayer.Username)
                            {
                                gameState.FilePath = file;
                                SavedGames.Add(gameState);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Ignorăm fișierele care nu pot fi deserializate
                    }
                }
            }

            // Sortăm jocurile după data salvării
            var sortedGames = SavedGames.OrderByDescending(g => g.SavedAt).ToList();
            SavedGames.Clear();

            foreach (var game in sortedGames)
            {
                SavedGames.Add(game);
            }

            if (SavedGames.Count == 0)
            {
                MessageBox.Show("Nu există jocuri salvate neterminate pentru utilizatorul curent.",
                              "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteGame(object parameter)
        {
            if (SelectedGame == null)
                return;

            var result = MessageBox.Show(
                $"Ești sigur că vrei să ștergi acest joc salvat?\nAceastă acțiune nu poate fi anulată!",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Ștergem fișierul de pe disc
                    File.Delete(SelectedGame.FilePath);

                    // Eliminăm jocul din lista afișată
                    SavedGames.Remove(SelectedGame);

                    MessageBox.Show("Jocul salvat a fost șters cu succes.",
                                   "Ștergere reușită",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la ștergerea jocului: {ex.Message}",
                                   "Eroare",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Error);
                }
            }
        }

        private void OpenGame(object parameter)
        {
            // Închide dialogul cu rezultat true (se va deschide jocul selectat)
            RequestClose?.Invoke(true);
        }

        private void Cancel(object parameter)
        {
            // Închide dialogul cu rezultat false
            RequestClose?.Invoke(false);
        }

        // Eveniment pentru a semnala închiderea dialogului
        public event Action<bool> RequestClose;

        public void RefreshGamesList()
        {
            LoadSavedGames();
        }
    }
}