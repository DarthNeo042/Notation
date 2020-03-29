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

            PeriodViewModel lastPeriod = null;
            foreach (PeriodViewModel period in MainViewModel.Instance.Parameters.Periods.OrderBy(p => p.Trimester).ThenBy(p => p.Number))
            {
                Periods.Add(period);
                if (lastPeriod == null)
                {
                    lastPeriod = period;
                }
                else
                {
                    if (lastPeriod.Trimester == period.Trimester)
                    {
                        SemiTrimesters.Add(new SemiTrimesterViewModel()
                        {
                            FromDate = lastPeriod.FromDate,
                            ToDate = period.ToDate,
                        });
                        lastPeriod = null;
                    }
                    else
                    {
                        SemiTrimesters.Add(new SemiTrimesterViewModel()
                        {
                            FromDate = period.FromDate,
                            ToDate = period.ToDate,
                        });
                        lastPeriod = period;
                    }
                }
            }
            if (lastPeriod != null)
            {
                SemiTrimesters.Add(new SemiTrimesterViewModel()
                {
                    FromDate = lastPeriod.FromDate,
                    ToDate = lastPeriod.ToDate,
                });
            }
            foreach (int trimester in Periods.Select(p => p.Trimester).Distinct())
            {
                Trimesters.Add(trimester);
            }
        }
    }
}
