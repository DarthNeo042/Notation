using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class ReportsViewModel : DependencyObject
    {
        public ObservableCollection<PeriodViewModel> Periods { get; set; }
        public ObservableCollection<SemiTrimesterViewModel> SemiTrimesters { get; set; }
        public ObservableCollection<int> Trimesters { get; set; }

        public ObservableCollection<string> PeriodReports { get; set; }

        public string SelectedPeriodReport
        {
            get { return (string)GetValue(SelectedPeriodReportProperty); }
            set { SetValue(SelectedPeriodReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriodReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodReportProperty =
            DependencyProperty.Register("SelectedPeriodReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string PeriodReportsPath
        {
            get { return (string)GetValue(PeriodReportsPathProperty); }
            set { SetValue(PeriodReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PeriodReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeriodReportsPathProperty =
            DependencyProperty.Register("PeriodReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenPeriodReportsPathCommand { get; set; }

        private void OpenPeriodReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(PeriodReportsPath);
        }

        private void OpenPeriodReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", string.Format("/root,{0}", PeriodReportsPath));
        }

        public ObservableCollection<string> SemiTrimesterReports { get; set; }

        public string SelectedSemiTrimesterReport
        {
            get { return (string)GetValue(SelectedSemiTrimesterReportProperty); }
            set { SetValue(SelectedSemiTrimesterReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSemiTrimesterReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSemiTrimesterReportProperty =
            DependencyProperty.Register("SelectedSemiTrimesterReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string SemiTrimesterReportsPath
        {
            get { return (string)GetValue(SemiTrimesterReportsPathProperty); }
            set { SetValue(SemiTrimesterReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SemiTrimesterReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SemiTrimesterReportsPathProperty =
            DependencyProperty.Register("SemiTrimesterReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenSemiTrimesterReportsPathCommand { get; set; }

        private void OpenSemiTrimesterReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(SemiTrimesterReportsPath);
        }

        private void OpenSemiTrimesterReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", string.Format("/root,{0}", SemiTrimesterReportsPath));
        }

        public ObservableCollection<string> TrimesterReports { get; set; }

        public string SelectedTrimesterReport
        {
            get { return (string)GetValue(SelectedTrimesterReportProperty); }
            set { SetValue(SelectedTrimesterReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTrimesterReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrimesterReportProperty =
            DependencyProperty.Register("SelectedTrimesterReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string TrimesterReportsPath
        {
            get { return (string)GetValue(TrimesterReportsPathProperty); }
            set { SetValue(TrimesterReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrimesterReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimesterReportsPathProperty =
            DependencyProperty.Register("TrimesterReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenTrimesterReportsPathCommand { get; set; }

        private void OpenTrimesterReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(TrimesterReportsPath);
        }

        private void OpenTrimesterReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", string.Format("/root,{0}", TrimesterReportsPath));
        }

        public CommandBindingCollection Bindings { get; set; }

        public ReportsViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>();
            Trimesters = new ObservableCollection<int>();

            PeriodReports = new ObservableCollection<string>();
            SemiTrimesterReports = new ObservableCollection<string>();
            TrimesterReports = new ObservableCollection<string>();

            OpenPeriodReportsPathCommand = new RoutedUICommand("OpenPeriodReportsPath", "OpenPeriodReportsPath", typeof(ReportsViewModel));
            OpenSemiTrimesterReportsPathCommand = new RoutedUICommand("OpenSemiTrimesterReportsPath", "OpenSemiTrimesterReportsPath", typeof(ReportsViewModel));
            OpenTrimesterReportsPathCommand = new RoutedUICommand("OpenTrimesterReportsPath", "OpenTrimesterReportsPath", typeof(ReportsViewModel));
            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(OpenPeriodReportsPathCommand, OpenPeriodReportsPathExecuted, OpenPeriodReportsPathCanExecute),
                new CommandBinding(OpenSemiTrimesterReportsPathCommand, OpenSemiTrimesterReportsPathExecuted, OpenSemiTrimesterReportsPathCanExecute),
                new CommandBinding(OpenTrimesterReportsPathCommand, OpenTrimesterReportsPathExecuted, OpenTrimesterReportsPathCanExecute),
            };
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
