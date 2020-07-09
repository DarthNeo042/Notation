using Notation.ViewModels;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour EntryPeriodComments.xaml
    /// </summary>
    public partial class EntryPeriodComments : Window
    {
        public EntryPeriodComments()
        {
            EntryPeriodCommentsViewModel entryPeriodComments = new EntryPeriodCommentsViewModel();
            DataContext = entryPeriodComments;
            InitializeComponent();
        }
    }
}
