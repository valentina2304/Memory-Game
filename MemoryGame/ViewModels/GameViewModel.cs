using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Views;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        // Proprietăți pentru utilizator și joc
        private User _currentPlayer;

        // Proprietăți pentru tipul de joc
        private bool _isStandardMode = true;
        private bool _isCustomMode = false;

        // Proprietăți pentru joc custom
        private int _customRows = 4;
        private int _customColumns = 4;
        private int _playerTimeSeconds = 60;

        // Proprietăți pentru categoria de imagini
        private bool _categoryOne = true;
        private bool _categoryTwo = false;
        private bool _categoryThree = false;

        // Proprietăți publice
        public User CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WinRateFormatted));
                OnPropertyChanged(nameof(HasNoRecentGames));
            }
        }


        // Proprietăți pentru tipul de joc
        public bool IsStandardMode
        {
            get => _isStandardMode;
            set
            {
                if (_isStandardMode != value)
                {
                    _isStandardMode = value;
                    if (value)
                    {
                        IsCustomMode = false;
                        // Resetăm valorile la cele standard
                        CustomRows = 4;
                        CustomColumns = 4;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCustomMode
        {
            get => _isCustomMode;
            set
            {
                if (_isCustomMode != value)
                {
                    _isCustomMode = value;
                    if (value)
                    {
                        IsStandardMode = false;
                    }
                    OnPropertyChanged();
                }
            }
        }
        private bool _isValidCardCount = true;

        public bool IsValidCardCount
        {
            get => _isValidCardCount;
            private set
            {
                if (_isValidCardCount != value)
                {
                    _isValidCardCount = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CardCountErrorMessage
        {
            get
            {
                if (IsValidCardCount)
                    return string.Empty;

                return "Numărul total de carduri trebuie să fie par";
            }
        }

        private void ValidateCardCount()
        {
            if (IsCustomMode)
            {
                int totalCards = CustomRows * CustomColumns;
                IsValidCardCount = (totalCards % 2 == 0);
            }
            else
            {
                IsValidCardCount = true;
            }
        }

        // Proprietăți pentru joc custom
        public int CustomRows
        {
            get => _customRows;
            set
            {
                if (value >= 2 && value <= 6) // Limitarea între 2 și 6
                {
                    _customRows = value;
                    OnPropertyChanged();
                    // Verificăm dacă numărul total de carduri este par
                    ValidateCardCount();
                    OnPropertyChanged(nameof(IsValidCardCount));
                    OnPropertyChanged(nameof(CardCountErrorMessage));
                }
            }
        }

        public int CustomColumns
        {
            get => _customColumns;
            set
            {
                if (value >= 2 && value <= 6) // Limitarea între 2 și 6
                {
                    _customColumns = value;
                    OnPropertyChanged();
                    // Verificăm dacă numărul total de carduri este par
                    ValidateCardCount();
                    OnPropertyChanged(nameof(IsValidCardCount));
                    OnPropertyChanged(nameof(CardCountErrorMessage));
                }
            }
        }
        public int PlayerTimeSeconds
        {
            get => _playerTimeSeconds;
            set
            {
                if (value > 0) // Timpul trebuie să fie pozitiv
                {
                    _playerTimeSeconds = value;
                    OnPropertyChanged();
                }
            }
        }

        // Proprietăți pentru categoria de imagini
        public bool CategoryOne
        {
            get => _categoryOne;
            set
            {
                if (_categoryOne != value)
                {
                    _categoryOne = value;
                    if (value)
                    {
                        CategoryTwo = false;
                        CategoryThree = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public bool CategoryTwo
        {
            get => _categoryTwo;
            set
            {
                if (_categoryTwo != value)
                {
                    _categoryTwo = value;
                    if (value)
                    {
                        CategoryOne = false;
                        CategoryThree = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public bool CategoryThree
        {
            get => _categoryThree;
            set
            {
                if (_categoryThree != value)
                {
                    _categoryThree = value;
                    if (value)
                    {
                        CategoryOne = false;
                        CategoryTwo = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        
        // Proprietăți calculate
        public string WinRateFormatted
        {
            get
            {
                if (CurrentPlayer == null || CurrentPlayer.GamesPlayed == 0)
                    return "0%";

                double winRate = (double)CurrentPlayer.GamesWon / CurrentPlayer.GamesPlayed * 100;
                return $"{winRate:F1}%";
            }
        }

        public bool HasNoRecentGames
        {
            get
            {
                return CurrentPlayer == null || CurrentPlayer.GamesPlayed == 0;
            }
        }

        // Comenzi
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ShowStatisticsCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand BackToLoginCommand { get; }

        // Constructor
        public GameViewModel()
        {
            // Inițializarea comenzilor
            NewGameCommand = new RelayCommand(_ => StartNewGame());
            OpenGameCommand = new RelayCommand(_ => OpenGame());
            SaveGameCommand = new RelayCommand(_ => SaveGame());
            ExitCommand = new RelayCommand(_ => BackToLogin());

            ShowStatisticsCommand = new RelayCommand(_ => ShowStatisticsWindow());

            ShowAboutCommand = new RelayCommand(_ => ShowAboutWindow());

            StartGameCommand = new RelayCommand(_ => StartGame(), _ => IsConfigurationValid());
            BackToLoginCommand = new RelayCommand(_ => BackToLogin());


            // Validăm configurația inițială
            ValidateCardCount();
        }

        private void ShowAboutWindow()
        {
            try
            {
                var aboutView = new AboutView();
                aboutView.Owner = Application.Current.MainWindow;
                aboutView.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la afișarea informațiilor About: {ex.Message}",
                              "Eroare",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void ShowStatisticsWindow()
        {
            try
            {
                var statisticsView = new StatisticsView();
                statisticsView.Owner = Application.Current.MainWindow;
                statisticsView.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la afișarea statisticilor: {ex.Message}",
                              "Eroare",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
         
        // Metodă pentru a reseta setările jocului
        private void ResetGameSettings()
        {
            IsStandardMode = true;
            IsCustomMode = false;
            CustomRows = 4;
            CustomColumns = 4;
            PlayerTimeSeconds = 60;
            CategoryOne = true;
            CategoryTwo = false;
            CategoryThree = false;
        }

        // Metoda pentru a deschide un joc salvat
        private void OpenGame()
        {
            try
            {
                // Verificăm dacă utilizatorul curent este selectat
                if (CurrentPlayer == null)
                {
                    MessageBox.Show("Trebuie să selectați un utilizator înainte de a deschide un joc salvat.",
                                    "Niciun utilizator selectat",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                // Deschidem dialogul personalizat pentru a alege un joc salvat, transmițând utilizatorul curent
                var openGameDialog = new OpenGameDialog(CurrentPlayer);

                if (openGameDialog.ShowDialog() == true && openGameDialog.SelectedGame != null)
                {
                    var selectedGame = openGameDialog.SelectedGame;

                    // Încărcăm jocul salvat - acum știm sigur că aparține utilizatorului curent
                    var gameBoardViewModel = GameBoardViewModel.LoadGame(selectedGame.FilePath);

                    if (gameBoardViewModel != null)
                    {
                        // Deschide fereastra jocului
                        var gameBoardView = new GameBoardView(gameBoardViewModel);
                        gameBoardView.Show();

                        // Închidem fereastra curentă
                        if (Application.Current.MainWindow is Window currentWindow)
                        {
                            Application.Current.MainWindow = gameBoardView;
                            currentWindow.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la deschiderea jocului: {ex.Message}",
                                "Eroare",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }


        // Metoda pentru a salva jocul curent
        private void SaveGame()
        {
            MessageBox.Show("Nu există niciun joc în desfășurare pentru a fi salvat. " +
                          "Începeți un joc nou înainte de a încerca să salvați.",
                          "Informație",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Metodă pentru a verifica dacă configurația jocului este validă
        private bool IsConfigurationValid()
        {
            // Verifică dacă avem un utilizator curent
            if (CurrentPlayer == null)
                return false;

            // Verifică configurația pentru joc custom
            if (IsCustomMode)
            {
                // Verifică dacă dimensiunile sunt între 2 și 6
                if (CustomRows < 2 || CustomRows > 6 || CustomColumns < 2 || CustomColumns > 6)
                    return false;

                // Verifică dacă numărul total de cartonașe este par
                if ((CustomRows * CustomColumns) % 2 != 0)
                    return false;
            }

            // Verifică dacă timpul per jucător este valid (pentru ambele tipuri de joc)
            if (PlayerTimeSeconds <= 0)
                return false;

            // Verifică dacă este selectată o categorie
            if (!CategoryOne && !CategoryTwo && !CategoryThree)
                return false;

            return true;
        }

        // Metoda pentru a începe un joc nou
        private void StartNewGame()
        {
            try
            {
                // Verificăm configurația
                if (!IsConfigurationValid())
                {
                    MessageBox.Show("Configurația jocului nu este validă. Vă rugăm verificați setările.",
                                  "Configurație invalidă",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                // Obținem categoria selectată
                int selectedCategory = 1;
                if (CategoryTwo) selectedCategory = 2;
                if (CategoryThree) selectedCategory = 3;

                // Obținem dimensiunile tablei de joc
                int rows = IsStandardMode ? 4 : CustomRows;
                int columns = IsStandardMode ? 4 : CustomColumns;

                // Obținem timpul per jucător
                int timePerPlayer = PlayerTimeSeconds;

                // Creăm view model-ul pentru tabla de joc
                var gameBoardViewModel = new GameBoardViewModel(CurrentPlayer, selectedCategory, rows, columns, timePerPlayer);

                // Deschidem fereastra jocului
                var gameBoardView = new GameBoardView(gameBoardViewModel);
                gameBoardView.Show();

                // Închidem fereastra curentă
                if (Application.Current.MainWindow is Window currentWindow)
                {
                    Application.Current.MainWindow = gameBoardView;
                    currentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la începerea jocului: {ex.Message}",
                              "Eroare",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        // Metoda pentru a începe jocul cu configurația selectată
        private void StartGame()
        {
            try
            {
                // Verificăm configurația
                if (!IsConfigurationValid())
                {
                    MessageBox.Show("Configurația jocului nu este validă. Vă rugăm verificați setările.",
                                  "Configurație invalidă",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                // Obținem categoria selectată
                int selectedCategory = 1;
                if (CategoryTwo) selectedCategory = 2;
                if (CategoryThree) selectedCategory = 3;

                // Obținem dimensiunile tablei de joc
                int rows = IsStandardMode ? 4 : CustomRows;
                int columns = IsStandardMode ? 4 : CustomColumns;

                // Obținem timpul per jucător
                int timePerPlayer = PlayerTimeSeconds;

                // Creăm view model-ul pentru tabla de joc
                var gameBoardViewModel = new GameBoardViewModel(CurrentPlayer, selectedCategory, rows, columns, timePerPlayer);

                // Deschidem fereastra jocului
                var gameBoardView = new GameBoardView(gameBoardViewModel);
                gameBoardView.Show();

                // Închidem fereastra curentă
                if (Application.Current.MainWindow is Window currentWindow)
                {
                    Application.Current.MainWindow = gameBoardView;
                    currentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la începerea jocului: {ex.Message}",
                              "Eroare",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        // Metoda pentru a reveni la ecranul de login
        private void BackToLogin()
        {
            try
            {
                // Creăm o nouă instanță a ferestrei de login
                var loginView = new LoginView();

                // Afișăm fereastra de login
                loginView.Show();

                // Închide fereastra curentă
                if (Application.Current.MainWindow is GameView gameView)
                {
                    // Setăm fereastra de login ca MainWindow
                    Application.Current.MainWindow = loginView;
                    gameView.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la revenirea la ecranul de login: {ex.Message}",
                               "Eroare",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        // Implementare INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}