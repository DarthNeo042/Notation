using Notation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour AddStudentsView.xaml
    /// </summary>
    public partial class AddStudents : Window
    {
        public ObservableCollection<StudentViewModel> Students { get; set; }

        public AddStudents(IEnumerable<StudentViewModel> students)
        {
            DataContext = this;

            Students = new ObservableCollection<StudentViewModel>(students);

            foreach (StudentViewModel student in Students)
            {
                student.Selected = false;
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
