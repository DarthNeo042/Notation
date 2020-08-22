using Notation.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class EntryViewModel : DependencyObject
    {
        public CommandBindingCollection Bindings { get; set; }

        public ICommand EntryMarksCommand { get; set; }

        private void EntryMarksCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainViewModel.Instance.Reports.Periods.Any();
        }

        private void EntryMarksExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntryMarks().ShowDialog();
        }

        public ICommand EntryPeriodCommentsCommand { get; set; }

        private void EntryPeriodCommentsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainViewModel.Instance.Reports.Periods.Any();
        }

        private void EntryPeriodCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntryPeriodComments().ShowDialog();
        }

        public ICommand EntrySemiTrimesterCommentsCommand { get; set; }

        private void EntrySemiTrimesterCommentsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainViewModel.Instance.Reports.SemiTrimesters.Any();
        }

        private void EntrySemiTrimesterCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntrySemiTrimesterComments().ShowDialog();
        }

        public ICommand EntryTrimesterSubjectCommentsCommand { get; set; }

        private void EntryTrimesterSubjectCommentsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainViewModel.Instance.Reports.Trimesters.Any();
        }

        private void EntryTrimesterSubjectCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntryTrimesterSubjectComments().ShowDialog();
        }

        public ICommand EntryTrimesterCommentsCommand { get; set; }

        private void EntryTrimesterCommentsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainViewModel.Instance.Reports.Trimesters.Any();
        }

        private void EntryTrimesterCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntryTrimesterComments().ShowDialog();
        }

        public EntryViewModel()
        {
            EntryMarksCommand = new RoutedUICommand("EntryMarks", "EntryMarks", typeof(MainViewModel));
            EntryPeriodCommentsCommand = new RoutedUICommand("EntryPeriodComments", "EntryPeriodComments", typeof(MainViewModel));
            EntrySemiTrimesterCommentsCommand = new RoutedUICommand("EntrySemiTrimesterComments", "EntrySemiTrimesterComments", typeof(MainViewModel));
            EntryTrimesterSubjectCommentsCommand = new RoutedUICommand("EntryTrimesterSubjectComments", "EntryTrimesterSubjectComments", typeof(MainViewModel));
            EntryTrimesterCommentsCommand = new RoutedUICommand("EntryTrimesterComments", "EntryTrimesterComments", typeof(MainViewModel));

            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(EntryMarksCommand, EntryMarksExecuted, EntryMarksCanExecute),
                new CommandBinding(EntryPeriodCommentsCommand, EntryPeriodCommentsExecuted, EntryPeriodCommentsCanExecute),
                new CommandBinding(EntrySemiTrimesterCommentsCommand, EntrySemiTrimesterCommentsExecuted, EntrySemiTrimesterCommentsCanExecute),
                new CommandBinding(EntryTrimesterSubjectCommentsCommand, EntryTrimesterSubjectCommentsExecuted, EntryTrimesterSubjectCommentsCanExecute),
                new CommandBinding(EntryTrimesterCommentsCommand, EntryTrimesterCommentsExecuted, EntryTrimesterCommentsCanExecute),
            };
        }
    }
}
