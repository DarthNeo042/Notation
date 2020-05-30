using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class EntryMarksViewModel : DependencyObject
    {
        public ObservableCollection<ClassViewModel> Classes { get; set; }

        public ClassViewModel SelectedClass
        {
            get { return (ClassViewModel)GetValue(SelectedClassProperty); }
            set { SetValue(SelectedClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClassProperty =
            DependencyProperty.Register("SelectedClass", typeof(ClassViewModel), typeof(EntryMarksViewModel), new PropertyMetadata(null));

        public ObservableCollection<PeriodViewModel> Periods { get; set; }

        public PeriodViewModel SelectedPeriod
        {
            get { return (PeriodViewModel)GetValue(SelectedPeriodProperty); }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodProperty =
            DependencyProperty.Register("SelectedPeriod", typeof(PeriodViewModel), typeof(EntryMarksViewModel), new PropertyMetadata(null));

        public EntryMarksViewModel()
        {
            Classes = new ObservableCollection<ClassViewModel>(MainViewModel.Instance.Parameters.Classes);
            Periods = new ObservableCollection<PeriodViewModel>(MainViewModel.Instance.Parameters.Periods);
        }
    }
}
