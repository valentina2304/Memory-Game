using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Clasă de bază pentru toate ViewModels, implementând notificarea de schimbare a proprietății
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Eveniment care este declanșat când o proprietate se modifică
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metodă helper pentru a declanșa evenimentul PropertyChanged
        /// </summary>
        /// <param name="propertyName">Numele proprietății care s-a schimbat</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Helper pentru a seta o proprietate și a notifica schimbarea dacă valoarea s-a modificat
        /// </summary>
        /// <typeparam name="T">Tipul proprietății</typeparam>
        /// <param name="storage">Câmpul de stocare a proprietății</param>
        /// <param name="value">Noua valoare</param>
        /// <param name="propertyName">Numele proprietății</param>
        /// <returns>True dacă valoarea s-a schimbat, false în caz contrar</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}