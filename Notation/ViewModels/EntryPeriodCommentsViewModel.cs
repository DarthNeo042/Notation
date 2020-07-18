using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class EntryPeriodCommentsViewModel : DependencyObject
    {
        public delegate void SelectedClassChangedEventHandler();

        public event SelectedClassChangedEventHandler SelectedClassChangedEvent;

        public ObservableCollection<EntryClassViewModel> Classes { get; set; }

        public EntryClassViewModel SelectedClass
        {
            get { return (EntryClassViewModel)GetValue(SelectedClassProperty); }
            set { SetValue(SelectedClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClassProperty =
            DependencyProperty.Register("SelectedClass", typeof(EntryClassViewModel), typeof(EntryPeriodCommentsViewModel), new PropertyMetadata(null, SelectedClassChanged));

        private static void SelectedClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryPeriodCommentsViewModel entryPeriodComments = (EntryPeriodCommentsViewModel)d;
            entryPeriodComments.SelectedClassChangedEvent?.Invoke();
        }

        public ObservableCollection<PeriodViewModel> Periods { get; set; }

        public PeriodViewModel SelectedPeriod
        {
            get { return (PeriodViewModel)GetValue(SelectedPeriodProperty); }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodProperty =
            DependencyProperty.Register("SelectedPeriod", typeof(PeriodViewModel), typeof(EntryPeriodCommentsViewModel), new PropertyMetadata(null));

        public CommandBindingCollection Bindings { get; set; }

        public EntryPeriodCommentsViewModel()
        {
            Classes = new ObservableCollection<EntryClassViewModel>();
            Periods = new ObservableCollection<PeriodViewModel>(MainViewModel.Instance.Parameters.Periods);

            SelectedPeriod = Periods.FirstOrDefault(p => p.FromDate <= DateTime.Now.Date && p.ToDate > DateTime.Now.Date.AddDays(1));
            if (SelectedPeriod == null)
            {
                SelectedPeriod = Periods.FirstOrDefault();
            }

            Load();
        }

        public void Load()
        {
            Classes.Clear();
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            {
                EntryClassViewModel entryClass = new EntryClassViewModel() { Class = _class };
                foreach (StudentViewModel student in _class.Students)
                {
                    EntryStudentViewModel entryStudent = new EntryStudentViewModel() { Student = student };
                    entryStudent.LoadEntryPeriodComments();
                    entryClass.Students.Add(entryStudent);
                }
                Classes.Add(entryClass);
            }

            SelectedClass = Classes.FirstOrDefault();
            if (SelectedClass != null)
            {
                SelectedClass.SelectedStudent = SelectedClass.Students.FirstOrDefault();
            }
        }
    }
}
