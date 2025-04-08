using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MemoryGame.ViewModels
{
   
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly UserService _userService;

        public ObservableCollection<UserWithStats> Users { get; } = new ObservableCollection<UserWithStats>();

        public RelayCommand CloseCommand { get; }

        public StatisticsViewModel()
        {
            _userService = new UserService();
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());

            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userService.LoadUsers();

                var usersWithStats = users.Select(u => new UserWithStats(u)).ToList();

                usersWithStats = usersWithStats.OrderByDescending(u => u.WinRate).ToList();

                Users.Clear();
                foreach (var user in usersWithStats)
                {
                    Users.Add(user);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Eroare la încărcarea statisticilor: {ex.Message}",
                                              "Eroare",
                                              System.Windows.MessageBoxButton.OK,
                                              System.Windows.MessageBoxImage.Error);
            }
        }

        public event Action RequestClose;
    }

   
    public class UserWithStats : User
    {
        public double WinRate
        {
            get
            {
                if (GamesPlayed == 0)
                    return 0;

                return (double)GamesWon / GamesPlayed * 100;
            }
        }

        public UserWithStats(User user)
        {
            Username = user.Username;
            ImagePath = user.ImagePath;
            GamesPlayed = user.GamesPlayed;
            GamesWon = user.GamesWon;
        }
    }
}