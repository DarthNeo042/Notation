using Notation.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateYear.xaml
    /// </summary>
    public partial class CreateYear : Window
    {
        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Year.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(CreateYear), new PropertyMetadata(0));

        public ObservableCollection<int> CopyYears { get; set; }

        public int SelectedCopyYear
        {
            get { return (int)GetValue(SelectedCopyYearProperty); }
            set { SetValue(SelectedCopyYearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCopyYear.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCopyYearProperty =
            DependencyProperty.Register("SelectedCopyYear", typeof(int), typeof(CreateYear), new PropertyMetadata(0));

        public CreateYear()
        {
            CopyYears = new ObservableCollection<int>(MainViewModel.Instance.Years);
            if (MainViewModel.Instance.SelectedYear != 0)
            {
                SelectedCopyYear = MainViewModel.Instance.SelectedYear;
                Year = SelectedCopyYear + 1;
                while (CopyYears.Contains(Year))
                {
                    Year++;
                }
            }
            else
            {
                Year = DateTime.Now.Year;
            }

            DataContext = this;

            InitializeComponent();
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Voulez-vous créer l'année {Year}/{Year + 1} ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DialogResult = true;
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            Year--;
            Validate.IsEnabled = !CopyYears.Contains(Year);
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Year++;
            Validate.IsEnabled = !CopyYears.Contains(Year);
        }
    }
}
