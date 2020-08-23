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
            Process.Start("explorer", $"/root,{PeriodModelsPath}");
        }

        public ObservableCollection<string> SemiTrimesterModels { get; set; }

        public string SelectedSemiTrimesterModel
        {
            get { return (string)GetValue(SelectedSemiTrimesterModelProperty); }
            set { SetValue(SelectedSemiTrimesterModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSemiTrimesterModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSemiTrimesterModelProperty =
            DependencyProperty.Register("SelectedSemiTrimesterModel", typeof(string), typeof(ModelsViewModel), new PropertyMetadata(""));

        public string SemiTrimesterModelsPath
        {
            get { return (string)GetValue(SemiTrimesterModelsPathProperty); }
            set { SetValue(SemiTrimesterModelsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SemiTrimesterModelsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SemiTrimesterModelsPathProperty =
            DependencyProperty.Register("SemiTrimesterModelsPath", typeof(string), typeof(ModelsViewModel), new PropertyMetadata(""));

        public bool SuccessfulImport
        {
            get { return (bool)GetValue(SuccessfulImportProperty); }
            set { SetValue(SuccessfulImportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuccessfulImport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuccessfulImportProperty =
            DependencyProperty.Register("SuccessfulImport", typeof(bool), typeof(ModelsViewModel), new PropertyMetadata(false));

        public ICommand OpenSemiTrimesterModelsPathCommand { get; set; }

        private void OpenSemiTrimesterModelsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(SemiTrimesterModelsPath);
        }

        private void OpenSemiTrimesterModelsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", $"/root,{SemiTrimesterModelsPath}");
        }

        public ObservableCollection<string> TrimesterModels { get; set; }

        public string SelectedTrimesterModel
        {
            get { return (string)GetValue(SelectedTrimesterModelProperty); }
            set { SetValue(SelectedTrimesterModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTrimesterModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrimesterModelProperty =
            DependencyProperty.Register("SelectedTrimesterModel", typeof(string), typeof(ModelsViewModel), new PropertyMetadata(""));

        public string TrimesterModelsPath
        {
            get { return (string)GetValue(TrimesterModelsPathProperty); }
            set { SetValue(TrimesterModelsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrimesterModelsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimesterModelsPathProperty =
            DependencyProperty.Register("TrimesterModelsPath", typeof(string), typeof(ModelsViewModel), new PropertyMetadata(""));

        public ICommand OpenTrimesterModelsPathCommand { get; set; }

        private void OpenTrimesterModelsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(TrimesterModelsPath);
        }

        private void OpenTrimesterModelsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", $"/root,{TrimesterModelsPath}");
        }

        public CommandBindingCollection Bindings { get; set; }

        public ModelsViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>();
            Trimesters = new ObservableCollection<int>();

            PeriodModels = new ObservableCollection<string>();
            SemiTrimesterModels = new ObservableCollection<string>();
            TrimesterModels = new ObservableCollection<string>();

            OpenPeriodModelsPathCommand = new RoutedUICommand("OpenPeriodModelsPath", "OpenPeriodModelsPath", typeof(ModelsViewModel));
            OpenSemiTrimesterModelsPathCommand = new RoutedUICommand("OpenSemiTrimesterModelsPath", "OpenSemiTrimesterModelsPath", typeof(ModelsViewModel));
            OpenTrimesterModelsPathCommand = new RoutedUICommand("OpenTrimesterModelsPath", "OpenTrimesterModelsPath", typeof(ModelsViewModel));
            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(OpenPeriodModelsPathCommand, OpenPeriodModelsPathExecuted, OpenPeriodModelsPathCanExecute),
                new CommandBinding(OpenSemiTrimesterModelsPathCommand, OpenSemiTrimesterModelsPathExecuted, OpenSemiTrimesterModelsPathCanExecute),
                new CommandBinding(OpenTrimesterModelsPathCommand, OpenTrimesterModelsPathExecuted, OpenTrimesterModelsPathCanExecute),
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
