using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Tema2MemoryGame.Models;

namespace Tema2MemoryGame.Services;

public static class UserService
{
    private static readonly string UsersFilePath = Path.Combine(Directory.GetCurrentDirectory(), "users.json");

    public static ObservableCollection<User> LoadUsers()
    {
        try
        {
            if (File.Exists(UsersFilePath))
            {
                var json = File.ReadAllText(UsersFilePath);
                return JsonSerializer.Deserialize<ObservableCollection<User>>(json) ?? new ObservableCollection<User>();
            }
        }
        catch
        {
            // If there's any error, return empty collection
        }
        return new ObservableCollection<User>();
    }

    public static void SaveUsers(ObservableCollection<User> users)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(users, options);
        File.WriteAllText(UsersFilePath, json);
    }
}