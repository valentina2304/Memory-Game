using MemoryGame.Commands;
using System;

namespace MemoryGame.ViewModels
{
   
    public class AboutViewModel : ViewModelBase
    {
        
        public RelayCommand CloseCommand { get; }

        public AboutViewModel()
        {
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        public event Action RequestClose;
    }
}