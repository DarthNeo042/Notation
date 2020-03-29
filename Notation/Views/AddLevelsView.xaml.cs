using Notation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour AddLevelsView.xaml
    /// </summary>
    public partial class AddLevelsView : Window
    {
        public ObservableCollection<LevelViewModel> Levels { get; set; }

        public AddLevelsView(IEnumerable<LevelViewModel> levels)
        {
            DataContext = this;

            Levels = new ObservableCollection<LevelViewModel>(levels);

            foreach (LevelViewModel level in Levels)
            {
                level.Selected = false;
            }

            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
