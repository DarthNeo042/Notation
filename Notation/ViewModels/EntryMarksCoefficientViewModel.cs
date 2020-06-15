using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class EntryMarksCoefficientViewModel : DependencyObject
    {
        public double Coefficient
        {
            get { return (double)GetValue(CoefficientProperty); }
            set { SetValue(CoefficientProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Coefficient.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoefficientProperty =
            DependencyProperty.Register("Coefficient", typeof(double), typeof(EntryMarksCoefficientViewModel), new PropertyMetadata(0.0));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(EntryMarksCoefficientViewModel), new PropertyMetadata(""));

        public EntryMarksMarkViewModel SelectedMark
        {
            get { return (EntryMarksMarkViewModel)GetValue(SelectedMarkProperty); }
            set { SetValue(SelectedMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMarkProperty =
            DependencyProperty.Register("SelectedMark", typeof(EntryMarksMarkViewModel), typeof(EntryMarksMarkViewModel), new PropertyMetadata(null));

        public ObservableCollection<EntryMarksMarkViewModel> Marks { get; set; }

        public EntryMarksCoefficientViewModel()
        {
            Marks = new ObservableCollection<EntryMarksMarkViewModel>();
            Marks.Add(new EntryMarksMarkViewModel() { Mark = "1" });
            Marks.Add(new EntryMarksMarkViewModel() { Mark = "10" });
            Marks.Add(new EntryMarksMarkViewModel() { Mark = "19" });
            Marks.Add(new EntryMarksMarkViewModel() { Mark = "20" });
            Marks.Add(new EntryMarksMarkViewModel() { Mark = "" });
        }
    }
}
