using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class EntrySemiTrimesterCommentsViewModel : DependencyObject
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
            DependencyProperty.Register("SelectedClass", typeof(EntryClassViewModel), typeof(EntrySemiTrimesterCommentsViewModel), new PropertyMetadata(null, SelectedClassChanged));

        private static void SelectedClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntrySemiTrimesterCommentsViewModel entrySemiTrimesterComments = (EntrySemiTrimesterCommentsViewModel)d;
            entrySemiTrimesterComments.SelectedClassChangedEvent?.Invoke();
        }

        public ObservableCollection<SemiTrimesterViewModel> SemiTrimesters { get; set; }

        public SemiTrimesterViewModel SelectedSemiTrimester
        {
            get { return (SemiTrimesterViewModel)GetValue(SelectedSemiTrimesterProperty); }
            set { SetValue(SelectedSemiTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSemiTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSemiTrimesterProperty =
            DependencyProperty.Register("SelectedSemiTrimester", typeof(SemiTrimesterViewModel), typeof(EntrySemiTrimesterCommentsViewModel), new PropertyMetadata(null));

        public CommandBindingCollection Bindings { get; set; }

        public EntrySemiTrimesterCommentsViewModel()
        {
            Classes = new ObservableCollection<EntryClassViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>(MainViewModel.Instance.Parameters.SemiTrimesters);

            SelectedSemiTrimester = SemiTrimesters.FirstOrDefault(p => p.FromDate <= DateTime.Now.Date && p.ToDate > DateTime.Now.Date.AddDays(1));
            if (SelectedSemiTrimester == null)
            {
                SelectedSemiTrimester = SemiTrimesters.FirstOrDefault();
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
