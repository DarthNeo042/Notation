using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Notation.ViewModels
{
    public class ReportsViewModel : DependencyObject
    {
        public ObservableCollection<PeriodViewModel> Periods { get; set; }
        public ObservableCollection<SemiTrimesterViewModel> SemiTrimesters { get; set; }
        public ObservableCollection<int> Trimesters { get; set; }

        public ReportsViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>();
            Trimesters = new ObservableCollection<int>();
        }

        public void LoadData()
        {
            Periods.Clear();
            SemiTrimesters.Clear();
            Trimesters.Clear();

            foreach (PeriodViewModel period in MainViewModel.Instance.Parameters.Periods.OrderBy(p => p.Trimester).ThenBy(p => p.Number))
            {
                Periods.Add(period);
            }
            foreach (SemiTrimesterViewModel semiTrimester in MainViewModel.Instance.Parameters.SemiTrimesters)
            {
                SemiTrimesters.Add(semiTrimester);
            }
            foreach (int trimester in Periods.Select(p => p.Trimester).Distinct())
            {
                Trimesters.Add(trimester);
            }
        }
    }
}
