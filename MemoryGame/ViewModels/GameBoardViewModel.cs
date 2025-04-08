using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Views;
using Microsoft.Win32;

namespace MemoryGame.ViewModels
{
    public class GameBoardViewModel : ViewModelBase
    {

        private readonly UserService _userService;
        private readonly Random _random = new Random();
        private readonly List<string> _imageFiles = new List<string>();
        private DispatcherTimer _gameTimer;
        private Card _firstSelectedCard;
        private Card _secondSelectedCard;
        private bool _isCheckingMatch;
        private int _remainingTimeInSeconds;
        private bool _gameEnded;
        private string _loadedGameFilePath;



        public ObservableCollection<Card> Cards { get; } = new ObservableCollection<Card>();

        private User _currentPlayer;
        public User CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _category;
        public int Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _rows;
        public int Rows
        {
            get => _rows;
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _columns;
        public int Columns
        {
            get => _columns;
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _totalTimeInSeconds;
        public int TotalTimeInSeconds
        {
            get => _totalTimeInSeconds;
            set
            {
                if (_totalTimeInSeconds != value)
                {
                    _totalTimeInSeconds = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RemainingTimeInSeconds
        {
            get => _remainingTimeInSeconds;
            set
            {
                if (_remainingTimeInSeconds != value)
                {
                    _remainingTimeInSeconds = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TimeDisplay));
                }
            }
        }


        private string GetResourcePath(string folderName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string resourcePath = Path.Combine(baseDir, "Resources", folderName);

            if (!Directory.Exists(resourcePath))
            {
                Directory.CreateDirectory(resourcePath);
            }

            return resourcePath;
        }
        public bool GameEnded
        {
            get => _gameEnded;
            set
            {
                if (_gameEnded != value)
                {
                    _gameEnded = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TimeDisplay => $"{RemainingTimeInSeconds / 60:D2}:{RemainingTimeInSeconds % 60:D2}";



        public ICommand CardClickCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand ExitCommand { get; }



        public GameBoardViewModel(User player, int category, int rows, int columns, int timeInSeconds)
        {
            _userService = new UserService();
            CurrentPlayer = player ?? throw new ArgumentNullException(nameof(player));
            Category = category;
            Rows = rows;
            Columns = columns;
            TotalTimeInSeconds = timeInSeconds;
            RemainingTimeInSeconds = timeInSeconds;

            CardClickCommand = new RelayCommand(CardClick, CanCardClick);
            SaveGameCommand = new RelayCommand(_ => SaveGame(), _ => !GameEnded);
            ExitCommand = new RelayCommand(_ => Exit());

            LoadImages();
            InitializeCards();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                StartTimer();
            }), DispatcherPriority.Loaded);
        }



        private void LoadImages()
        {
            try
            {
                string categoryFolder;
                switch (Category)
                {
                    case 1:
                        categoryFolder = "Animals";
                        break;
                    case 2:
                        categoryFolder = "Food";
                        break;
                    case 3:
                        categoryFolder = "Travel";
                        break;
                    default:
                        categoryFolder = "Animals";
                        break;
                }

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string imagesFolder = Path.Combine(baseDir, "Resources", categoryFolder);

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                    MessageBox.Show($"Directorul pentru categoria {categoryFolder} a fost creat la locația: {imagesFolder}\nTe rugăm să adaugi imagini în acest director.",
                                   "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                string[] imageFiles = Directory.GetFiles(imagesFolder, "*.jpg")
                                        .Concat(Directory.GetFiles(imagesFolder, "*.png"))
                                        .Concat(Directory.GetFiles(imagesFolder, "*.gif"))
                                        .ToArray();

                if (imageFiles.Length == 0)
                {
                    MessageBox.Show($"Nu s-au găsit imagini în categoria {categoryFolder} la locația: {imagesFolder}",
                                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _imageFiles.AddRange(imageFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea imaginilor: {ex.Message}",
                                "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeCards()
        {
            Cards.Clear();

            int totalCards = Rows * Columns;
            int pairsNeeded = totalCards / 2;

            if (_imageFiles.Count < pairsNeeded && _imageFiles.Count > 0)
            {
                while (_imageFiles.Count < pairsNeeded)
                {
                    _imageFiles.AddRange(_imageFiles.Take(Math.Min(pairsNeeded - _imageFiles.Count, _imageFiles.Count)));
                }
            }
            else if (_imageFiles.Count == 0)
            {
                for (int i = 0; i < pairsNeeded; i++)
                {
                    _imageFiles.Add($"Value_{i}");
                }
            }

            var selectedImages = _imageFiles.Take(pairsNeeded).ToList();

            List<Card> allCards = new List<Card>();

            for (int i = 0; i < pairsNeeded; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    allCards.Add(new Card
                    {
                        Id = i * 2 + j,
                        ImagePath = selectedImages[i],
                        Value = i,
                        IsFlipped = false,
                        IsMatched = false
                    });
                }
            }

            ShuffleCards(allCards);

            foreach (var card in allCards)
            {
                Cards.Add(card);
            }
        }

        private void ShuffleCards(List<Card> cards)
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                Card temp = cards[k];
                cards[k] = cards[n];
                cards[n] = temp;
            }
        }

        private void StartTimer()
        {
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _gameTimer.Tick += (s, e) =>
            {
                if (RemainingTimeInSeconds > 0)
                {
                    RemainingTimeInSeconds--;
                }
                else
                {
                    _gameTimer.Stop();
                    EndGame(false);
                }
            };

            _gameTimer.Start();
        }
        public void StopTimer()
        {
            if (_gameTimer != null)
            {
                _gameTimer.Stop();
            }
        }
        private void EndGame(bool isWin)
        {

            if (_gameTimer != null)
            {
                _gameTimer.Stop();
                _gameTimer = null; 
            }

            GameEnded = true;

            CurrentPlayer.GamesPlayed++;
            if (isWin)
                CurrentPlayer.GamesWon++;

            var users = _userService.LoadUsers();
            var userIndex = users.FindIndex(u => u.Username == CurrentPlayer.Username);
            if (userIndex >= 0)
            {
                users[userIndex] = CurrentPlayer;
                _userService.SaveUsers(users);
            }

            if (!string.IsNullOrEmpty(_loadedGameFilePath) && File.Exists(_loadedGameFilePath))
            {
                try
                {
                    File.Delete(_loadedGameFilePath);
                    _loadedGameFilePath = string.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eroare la ștergerea fișierului de salvare: {ex.Message}");
                }
            }

            string message = isWin
                ? $"Felicitări, {CurrentPlayer.Username}! Ai câștigat jocul!"
                : $"Timpul a expirat! Ai pierdut jocul, {CurrentPlayer.Username}.";

            MessageBox.Show(message, "Joc terminat", MessageBoxButton.OK, isWin ? MessageBoxImage.Information : MessageBoxImage.Exclamation);
        }

        private bool CanCardClick(object parameter)
        {
            if (parameter is Card card)
            {
                return !card.IsFlipped && !card.IsMatched && !_isCheckingMatch && !GameEnded;
            }
            return false;
        }

        private void CardClick(object parameter)
        {
            if (!(parameter is Card card) || _isCheckingMatch || GameEnded)
                return;

            if (card.IsFlipped || card.IsMatched)
                return;

            card.IsFlipped = true;

            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = card;
            }
            else
            {
                if (_firstSelectedCard == card)
                    return;

                _secondSelectedCard = card;
                _isCheckingMatch = true;

                
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CheckForMatch();
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void CheckForMatch()
        {
            if (_firstSelectedCard == null || _secondSelectedCard == null)
            {
                _isCheckingMatch = false;
                return;
            }

            bool isMatch = _firstSelectedCard.Value == _secondSelectedCard.Value;

            if (isMatch)
            {
                _firstSelectedCard.IsMatched = true;
                _secondSelectedCard.IsMatched = true;
               

                if (Cards.All(c => c.IsMatched))
                {
                    EndGame(true);
                }

                _firstSelectedCard = null;
                _secondSelectedCard = null;
                _isCheckingMatch = false;
            }
            else
            {
                var timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500) 
                };

                timer.Tick += (s, e) =>
                {
                    timer.Stop();

                    if (_firstSelectedCard != null && _secondSelectedCard != null)
                    {
                        _firstSelectedCard.IsFlipped = false;
                        _secondSelectedCard.IsFlipped = false;
                    }

                    _firstSelectedCard = null;
                    _secondSelectedCard = null;
                    _isCheckingMatch = false;
                };

                timer.Start();
            }
        }

        private void SaveGame()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string saveDir = Path.Combine(baseDir, "SavedGames");

                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                string fullPath;

                if (!string.IsNullOrEmpty(_loadedGameFilePath))
                {
                    fullPath = _loadedGameFilePath;
                }
                else
                {
                    string fileName = $"MemoryGame_{CurrentPlayer.Username}_{DateTime.Now:yyyyMMdd_HHmmss}.mem";
                    fullPath = Path.Combine(saveDir, fileName);
                }

                _gameTimer?.Stop();

                GameState gameState = new GameState
                {
                    PlayerName = CurrentPlayer.Username,
                    Category = Category,
                    Rows = Rows,
                    Columns = Columns,
                    TotalTime = TotalTimeInSeconds,
                    RemainingTime = RemainingTimeInSeconds,
                    SavedAt = DateTime.Now,
                    IsCompleted = GameEnded,
                    Cards = Cards.Select((card, index) => new CardState
                    {
                        Id = card.Id,
                        Value = card.Value,
                        ImagePath = card.ImagePath,
                        IsFlipped = card.IsFlipped,
                        IsMatched = card.IsMatched,
                        Position = index
                    }).ToList()
                };

                string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(fullPath, json);

                _loadedGameFilePath = fullPath;

                MessageBox.Show($"Jocul a fost salvat cu succes!",
                               "Salvare reușită", MessageBoxButton.OK, MessageBoxImage.Information);

                if (!GameEnded)
                {
                    _gameTimer?.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea jocului: {ex.Message}",
                                "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);

                if (!GameEnded)
                {
                    _gameTimer?.Start();
                }
            }
        }


        public static GameBoardViewModel LoadGame(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Fișierul de salvare nu există.",
                                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                string json = File.ReadAllText(filePath);
                GameState gameState = JsonSerializer.Deserialize<GameState>(json);

                if (gameState == null)
                {
                    MessageBox.Show("Fișierul de salvare este invalid.",
                                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                if (gameState.IsCompleted)
                {
                    MessageBox.Show("Acest joc este deja terminat și nu poate fi continuat.",
                                  "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                }

                var userService = new UserService();
                var users = userService.LoadUsers();
                var player = users.FirstOrDefault(u => u.Username == gameState.PlayerName);

                if (player == null)
                {
                    MessageBox.Show($"Utilizatorul {gameState.PlayerName} nu există. Vă rugăm să creați acest utilizator mai întâi.",
                                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                var viewModel = new GameBoardViewModel(player, gameState.Category, gameState.Rows, gameState.Columns, gameState.TotalTime);

                viewModel._loadedGameFilePath = filePath;

                viewModel._gameTimer?.Stop();

                viewModel.RemainingTimeInSeconds = gameState.RemainingTime;

                viewModel.Cards.Clear();

                foreach (var cardState in gameState.Cards.OrderBy(c => c.Position))
                {
                    viewModel.Cards.Add(new Card
                    {
                        Id = cardState.Id,
                        Value = cardState.Value,
                        ImagePath = cardState.ImagePath,
                        IsFlipped = cardState.IsFlipped,
                        IsMatched = cardState.IsMatched
                    });
                }

                bool allMatched = viewModel.Cards.All(c => c.IsMatched);
                if (!allMatched)
                {
                    viewModel._gameTimer?.Start();
                }
                else
                {
                    viewModel.GameEnded = true;
                }

                return viewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea jocului: {ex.Message}",
                                "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }


        private void Exit()
        {
            _gameTimer?.Stop();

            if (!GameEnded)
            {
                var result = MessageBox.Show("Doriți să salvați jocul înainte de a ieși?",
                                            "Salvare joc",
                                            MessageBoxButton.YesNoCancel,
                                            MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveGame();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    _gameTimer?.Start();
                    return;
                }
            }

            var loginView = new LoginView();
            loginView.Show();

            if (Application.Current.MainWindow is Window currentWindow)
            {
                Application.Current.MainWindow = loginView;
                currentWindow.Close();
            }
        }

    }
}