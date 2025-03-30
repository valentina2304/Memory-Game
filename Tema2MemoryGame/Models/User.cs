using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tema2MemoryGame.Models;

public class User(string? username, string? avatarPath) : INotifyPropertyChanged
{
    private string? _username = username;
    private string? _avatarPath = avatarPath;

    public string? Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    public string? AvatarPath
    {
        get => _avatarPath;
        set
        {
            if (_avatarPath != value)
            {
                _avatarPath = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}