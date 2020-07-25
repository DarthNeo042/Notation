using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class EntryTrimesterCommentsViewModel : DependencyObject
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
            DependencyProperty.Register("SelectedClass", typeof(EntryClassViewModel), typeof(EntryTrimesterCommentsViewModel), new PropertyMetadata(null, SelectedClassChanged));

        private static void SelectedClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryTrimesterCommentsViewModel entryTrimesterComments = (EntryTrimesterCommentsViewModel)d;
            entryTrimesterComments.SelectedClassChangedEvent?.Invoke();
        }

        public ObservableCollection<int> Trimesters { get; set; }

        public int SelectedTrimester
        {
            get { return (int)GetValue(SelectedTrimesterProperty); }
            set { SetValue(SelectedTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrimesterProperty =
            DependencyProperty.Register("SelectedTrimester", typeof(int), typeof(EntryTrimesterCommentsViewModel), new PropertyMetadata(0, SelectedTrimesterChanged));

        private static void SelectedTrimesterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryTrimesterCommentsViewModel entryTrimesterComments = (EntryTrimesterCommentsViewModel)d;
            entryTrimesterComments.FromDate = MainViewModel.Instance.Parameters.Periods.Where(p => p.Trimester == entryTrimesterComments.SelectedTrimester).OrderBy(p => p.Number).First().FromDate;
            entryTrimesterComments.ToDate = MainViewModel.Instance.Parameters.Periods.Where(p => p.Trimester == entryTrimesterComments.SelectedTrimester).OrderByDescending(p => p.Number).First().ToDate;
        }

        public DateTime FromDate
        {
            get { return (DateTime)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime), typeof(EntryTrimesterCommentsViewModel), new PropertyMetadata(DateTime.Now));

        public DateTime ToDate
        {
            get { return (DateTime)GetValue(ToDateProperty); }
            set { SetValue(ToDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToDateProperty =
            DependencyProperty.Register("ToDate", typeof(DateTime), typeof(EntryTrimesterCommentsViewModel), new PropertyMetadata(DateTime.Now));

        public CommandBindingCollection Bindings { get; set; }

        public EntryTrimesterCommentsViewModel()
        {
            Classes = new ObservableCollection<EntryClassViewModel>();
            Trimesters = new ObservableCollection<int>(MainViewModel.Instance.Parameters.Periods.Select(p => p.Trimester).Distinct());

            PeriodViewModel period = MainViewModel.Instance.Parameters.Periods.FirstOrDefault(p => p.FromDate <= DateTime.Now.Date && p.ToDate > DateTime.Now.Date.AddDays(1));
            SelectedTrimester = period != null ? period.Trimester : 1;

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
