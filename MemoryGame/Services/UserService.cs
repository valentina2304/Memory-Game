using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Models;
using System.IO;
using System.Text.Json;

namespace MemoryGame.Services
{
    public class UserService
    {
        private const string UsersFileName = "users.json";
        private string _filePath;

        public UserService()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(baseDir, UsersFileName);
        }

        public List<User> LoadUsers()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            string json = File.ReadAllText(_filePath);
            var users = string.IsNullOrEmpty(json) ? new List<User>() : JsonSerializer.Deserialize<List<User>>(json);

            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.ImagePath) && !Path.IsPathRooted(user.ImagePath))
                {
                    user.ImagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, user.ImagePath));
                }
            }

            return users;
        }

        public void SaveUsers(List<User> users)
        {
            var usersToSave = new List<User>();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var user in users)
            {
                var userCopy = new User
                {
                    Username = user.Username,
                    GamesPlayed = user.GamesPlayed,
                    GamesWon = user.GamesWon
                };

                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    if (Path.IsPathRooted(user.ImagePath) && user.ImagePath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
                    {
                        userCopy.ImagePath = user.ImagePath.Substring(baseDir.Length);

                        if (userCopy.ImagePath.StartsWith("\\") || userCopy.ImagePath.StartsWith("/"))
                        {
                            userCopy.ImagePath = userCopy.ImagePath.Substring(1);
                        }
                    }
                    else
                    {
                        userCopy.ImagePath = user.ImagePath;
                    }
                }

                usersToSave.Add(userCopy);
            }

            string json = JsonSerializer.Serialize(usersToSave, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        public void AddUser(User newUser)
        {
            var users = LoadUsers();

            if (users.Any(u => u.Username == newUser.Username))
                throw new Exception("Utilizatorul există deja!");

            users.Add(newUser);
            SaveUsers(users);
        }

        public void DeleteUser(string username)
        {
            var users = LoadUsers();
            users.RemoveAll(u => u.Username == username);
            SaveUsers(users);
        }
        public bool UserExists(string username)
        {
            var users = LoadUsers();
            return users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
