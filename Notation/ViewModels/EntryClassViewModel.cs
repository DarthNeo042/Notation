using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class EntryClassViewModel : DependencyObject
    {
        public delegate void SelectedStudentChangedEventHandler();

        public event SelectedStudentChangedEventHandler SelectedStudentChangedEvent;

        public bool SelectedStudentChangedSet
        {
            get
            {
                return SelectedStudentChangedEvent != null;
            }
        }

        public ClassViewModel Class
        {
            get { return (ClassViewModel)GetValue(ClassProperty); }
            set { SetValue(ClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Class.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClassProperty =
            DependencyProperty.Register("Class", typeof(ClassViewModel), typeof(EntryClassViewModel), new PropertyMetadata(null));

        public EntryStudentViewModel SelectedStudent
        {
            get { return (EntryStudentViewModel)GetValue(SelectedStudentProperty); }
            set { SetValue(SelectedStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStudentProperty =
            DependencyProperty.Register("SelectedStudent", typeof(EntryStudentViewModel), typeof(EntryClassViewModel), new PropertyMetadata(null, SelectedStudentChanged));

        private static void SelectedStudentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryClassViewModel entryClass = (EntryClassViewModel)d;
            entryClass.SelectedStudentChangedEvent?.Invoke();
        }

        public ObservableCollection<EntryStudentViewModel> Students { get; set; }

        public EntryClassViewModel()
        {
            Students = new ObservableCollection<EntryStudentViewModel>();
        }
    }
}
