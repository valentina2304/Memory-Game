using MemoryGame.ViewModels;
using System.Windows;

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : Window
    {
        private StatisticsViewModel _viewModel;

        public StatisticsView()
        {
            InitializeComponent();

            _viewModel = new StatisticsViewModel();
            _viewModel.RequestClose += () => Close();
            DataContext = _viewModel;
        }
    }
}