using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// ViewModel pentru afișarea statisticilor jucătorilor
    /// </summary>
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly UserService _userService;

        /// <summary>
        /// Lista utilizatorilor cu statistici
        /// </summary>
        public ObservableCollection<UserWithStats> Users { get; } = new ObservableCollection<UserWithStats>();

        /// <summary>
        /// Comanda pentru închiderea ferestrei
        /// </summary>
        public RelayCommand CloseCommand { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StatisticsViewModel()
        {
            _userService = new UserService();
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());

            LoadUsers();
        }

        /// <summary>
        /// Încarcă lista utilizatorilor și statisticile acestora
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                // Obținem toți utilizatorii
                var users = _userService.LoadUsers();

                // Convertim utilizatorii în UserWithStats pentru a avea proprietatea WinRate
                var usersWithStats = users.Select(u => new UserWithStats(u)).ToList();

                // Sortăm lista după rata de câștig (descrescător)
                usersWithStats = usersWithStats.OrderByDescending(u => u.WinRate).ToList();

                // Adăugăm utilizatorii în colecția observabilă
                Users.Clear();
                foreach (var user in usersWithStats)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                // Tratăm posibile erori la încărcarea utilizatorilor
                System.Windows.MessageBox.Show($"Eroare la încărcarea statisticilor: {ex.Message}",
                                              "Eroare",
                                              System.Windows.MessageBoxButton.OK,
                                              System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Eveniment pentru a semnala închiderea ferestrei
        /// </summary>
        public event Action RequestClose;
    }

    /// <summary>
    /// Clasă utilizată pentru a extinde User cu proprietatea calculată WinRate
    /// </summary>
    public class UserWithStats : User
    {
        /// <summary>
        /// Rata de câștig a jucătorului (procent)
        /// </summary>
        public double WinRate
        {
            get
            {
                if (GamesPlayed == 0)
                    return 0;

                return (double)GamesWon / GamesPlayed * 100;
            }
        }

        /// <summary>
        /// Constructor care preia datele dintr-un User existent
        /// </summary>
        /// <param name="user">Utilizatorul sursă</param>
        public UserWithStats(User user)
        {
            Username = user.Username;
            ImagePath = user.ImagePath;
            GamesPlayed = user.GamesPlayed;
            GamesWon = user.GamesWon;
        }
    }
}