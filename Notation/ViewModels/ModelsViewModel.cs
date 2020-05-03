using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class ModelsViewModel : DependencyObject
    {
        public ObservableCollection<PeriodViewModel> Periods { get; set; }
        public ObservableCollection<SemiTrimesterViewModel> SemiTrimesters { get; set; }
        public ObservableCollection<int> Trimesters { get; set; }

        public ObservableCollection<string> PeriodModels { get; set; }

        public string SelectedPeriodModel
        {
            get { return (string)GetValue(SelectedPeriodModelProperty); }
            set { SetValue(SelectedPeriodModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriodModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodModelProperty =
            DependencyProperty.Register("SelectedPeriodModel", typeof(string), typeof(ModelsViewModel), new PropertyMetadata(""));

        public string PeriodModelsPath
        {
            get { return (string)GetValue(PeriodModelsPathProperty); }
            set { SetValue(PeriodModelsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PeriodModelsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeriodModelsPathProperty =
            DependencyProperty.Register("PeriodModelsPath", typeof(string), typeof(ModelsViewModel), new PropertyMetadata(""));

        public ICommand OpenPeriodModelsPathCommand { get; set; }

        private void OpenPeriodModelsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(PeriodModelsPath);
        }

        private void OpenPeriodModelsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", string.Format("/root,{0}", PeriodModelsPath));
        }

        public CommandBindingCollection Bindings { get; set; }

        public ModelsViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>();
            Trimesters = new ObservableCollection<int>();

            PeriodModels = new ObservableCollection<string>();

            OpenPeriodModelsPathCommand = new RoutedUICommand("OpenPeriodModelsPath", "OpenPeriodModelsPath", typeof(ModelsViewModel));
            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(OpenPeriodModelsPathCommand, OpenPeriodModelsPathExecuted, OpenPeriodModelsPathCanExecute),
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
            foreach (SemiTrimesterViewModel semiTrimester in MainViewModel.Instance.Parameters.SemiTrimesters.OrderBy(s => s.FromDate))
            {
                SemiTrimesters.Add(semiTrimester);
            }
            foreach (int trimester in Periods.Select(p => p.Trimester).Distinct().OrderBy(t => t))
            {
                Trimesters.Add(trimester);
            }
        }
    }
}
