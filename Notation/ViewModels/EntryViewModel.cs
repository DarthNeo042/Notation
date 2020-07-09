using Notation.Views;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class EntryViewModel : DependencyObject
    {
        public CommandBindingCollection Bindings { get; set; }

        public ICommand EntryMarksCommand { get; set; }

        private void EntryMarksExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntryMarks().ShowDialog();
        }

        public ICommand EntryPeriodCommentsCommand { get; set; }

        private void EntryPeriodCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new EntryPeriodComments().ShowDialog();
        }

        public ICommand EntrySemiTrimesterCommentsCommand { get; set; }

        private void EntrySemiTrimesterCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        public ICommand EntryTrimesterSubjectCommentsCommand { get; set; }

        private void EntryTrimesterSubjectCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        public ICommand EntryTrimesterCommentsCommand { get; set; }

        private void EntryTrimesterCommentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
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
                new CommandBinding(EntryMarksCommand, EntryMarksExecuted),
                new CommandBinding(EntryPeriodCommentsCommand, EntryPeriodCommentsExecuted),
                new CommandBinding(EntrySemiTrimesterCommentsCommand, EntrySemiTrimesterCommentsExecuted),
                new CommandBinding(EntryTrimesterSubjectCommentsCommand, EntryTrimesterSubjectCommentsExecuted),
                new CommandBinding(EntryTrimesterCommentsCommand, EntryTrimesterCommentsExecuted),
            };
        }
    }
}
