using Notation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour AddTeachersView.xaml
    /// </summary>
    public partial class AddTeachers : Window
    {
        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public AddTeachers(IEnumerable<TeacherViewModel> teachers)
        {
            DataContext = this;

            Teachers = new ObservableCollection<TeacherViewModel>(teachers);

            foreach (TeacherViewModel teacher in Teachers)
            {
                teacher.Selected = false;
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
