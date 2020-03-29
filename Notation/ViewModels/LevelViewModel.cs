using Notation.Utils;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class LevelViewModel : BaseViewModel
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(LevelViewModel), new PropertyMetadata("", NamePropertyChanged));

        private static void NamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelViewModel level = (LevelViewModel)d;
            level.Name = NameUtils.FormatPascal(level.Name);
        }

        public SubjectViewModel SelectedSubject
        {
            get { return (SubjectViewModel)GetValue(SelectedSubjectProperty); }
            set { SetValue(SelectedSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSubjectProperty =
            DependencyProperty.Register("SelectedSubject", typeof(SubjectViewModel), typeof(LevelViewModel), new PropertyMetadata(null));

        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public LevelViewModel()
        {
            Subjects = new ObservableCollection<SubjectViewModel>();
        }
    }
}
