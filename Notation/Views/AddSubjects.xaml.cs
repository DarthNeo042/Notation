using Notation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour AddSubjectsView.xaml
    /// </summary>
    public partial class AddSubjects : Window
    {
        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public AddSubjects(IEnumerable<SubjectViewModel> subjects)
        {
            DataContext = this;

            Subjects = new ObservableCollection<SubjectViewModel>(subjects);

            foreach (SubjectViewModel subject in Subjects)
            {
                subject.Selected = false;
            }

            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
