using Notation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour AddClassesView.xaml
    /// </summary>
    public partial class AddClasses : Window
    {
        public ObservableCollection<ClassViewModel> Classes { get; set; }

        public AddClasses(IEnumerable<ClassViewModel> classes)
        {
            DataContext = this;

            Classes = new ObservableCollection<ClassViewModel>(classes);

            foreach (ClassViewModel _class in Classes)
            {
                _class.Selected = false;
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
