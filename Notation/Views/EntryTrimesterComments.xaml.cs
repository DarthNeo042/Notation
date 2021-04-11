using Notation.Models;
using Notation.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour InputTrimesterComments.xaml
    /// </summary>
    public partial class EntryTrimesterComments : Window
    {
        public EntryTrimesterComments()
        {
            EntryTrimesterCommentsViewModel entryTrimesterComments = new EntryTrimesterCommentsViewModel();
            DataContext = entryTrimesterComments;
            InitializeComponent();

            entryTrimesterComments.SelectedClassChangedEvent += EntryTrimesterComments_SelectedClassChangedEvent;
            EntryTrimesterComments_SelectedClassChangedEvent();
            SelectedClass_SelectedStudentChangedEvent();

            ListView_SelectionChanged(null, null);
        }

        private void EntryTrimesterComments_SelectedClassChangedEvent()
        {
            EntryTrimesterCommentsViewModel entryTrimesterComments = (EntryTrimesterCommentsViewModel)DataContext;
            if (entryTrimesterComments.SelectedClass != null && !entryTrimesterComments.SelectedClass.SelectedStudentChangedSet)
            {
                entryTrimesterComments.SelectedClass.SelectedStudentChangedEvent += SelectedClass_SelectedStudentChangedEvent;
                entryTrimesterComments.SelectedClass.SelectedStudent = entryTrimesterComments.SelectedClass.Students.FirstOrDefault();
            }
            SelectedClass_SelectedStudentChangedEvent();
        }

        private void SelectedClass_SelectedStudentChangedEvent()
        {
            EntryTrimesterCommentsViewModel entryTrimesterComments = (EntryTrimesterCommentsViewModel)DataContext;

            TrimesterCommentModel TrimesterComment = TrimesterCommentModel.Read(entryTrimesterComments.SelectedTrimester, entryTrimesterComments.SelectedClass.SelectedStudent.Student);
            if (TrimesterComment != null)
            {
                MainTeacherCommentTextBox.Text = TrimesterComment.MainTeacherComment;
                DivisionPrefectCommentTextBox.Text = TrimesterComment.DivisionPrefectComment;
            }
            else
            {
                MainTeacherCommentTextBox.Text = "";
                DivisionPrefectCommentTextBox.Text = "";
            }
        }

        private void TrimesterComment_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveTrimesterComments((EntryTrimesterCommentsViewModel)DataContext);
        }

        private void TrimesterComment_KeyDown(object sender, KeyEventArgs e)
        {
            EntryTrimesterCommentsViewModel entryTrimesterComments = (EntryTrimesterCommentsViewModel)DataContext;

            switch (e.Key)
            {
                case Key.Down:
                    {
                        TextBox textBox = (TextBox)sender;
                        if (textBox == MainTeacherCommentTextBox)
                        {
                            DivisionPrefectCommentTextBox.Focus();
                        }
                        else if (textBox == DivisionPrefectCommentTextBox)
                        {
                            SaveTrimesterComments(entryTrimesterComments);
                            if (entryTrimesterComments.SelectedClass.SelectedStudent != entryTrimesterComments.SelectedClass.Students.Last())
                            {
                                entryTrimesterComments.SelectedClass.SelectedStudent
                                    = entryTrimesterComments.SelectedClass.Students[entryTrimesterComments.SelectedClass.Students.IndexOf(entryTrimesterComments.SelectedClass.SelectedStudent) + 1];
                            }
                            else
                            {
                                if (entryTrimesterComments.SelectedClass != entryTrimesterComments.Classes.Last())
                                {
                                    entryTrimesterComments.SelectedClass = entryTrimesterComments.Classes[entryTrimesterComments.Classes.IndexOf(entryTrimesterComments.SelectedClass) + 1];
                                }
                                else
                                {
                                    MessageBox.Show("Fin de la saisie.", "Fin", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Up:
                    {
                        TextBox textBox = (TextBox)sender;
                        if (textBox == DivisionPrefectCommentTextBox)
                        {
                            MainTeacherCommentTextBox.Focus();
                        }
                        else if (textBox == MainTeacherCommentTextBox)
                        {
                            SaveTrimesterComments(entryTrimesterComments);
                            if (entryTrimesterComments.SelectedClass.SelectedStudent != entryTrimesterComments.SelectedClass.Students.First())
                            {
                                entryTrimesterComments.SelectedClass.SelectedStudent
                                    = entryTrimesterComments.SelectedClass.Students[entryTrimesterComments.SelectedClass.Students.IndexOf(entryTrimesterComments.SelectedClass.SelectedStudent) - 1];
                            }
                            else
                            {
                                if (entryTrimesterComments.SelectedClass != entryTrimesterComments.Classes.First())
                                {
                                    entryTrimesterComments.SelectedClass = entryTrimesterComments.Classes[entryTrimesterComments.Classes.IndexOf(entryTrimesterComments.SelectedClass) - 1];
                                    entryTrimesterComments.SelectedClass.SelectedStudent = entryTrimesterComments.SelectedClass.Students.FirstOrDefault();
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void SaveTrimesterComments(EntryTrimesterCommentsViewModel entryTrimesterComments)
        {
            TrimesterCommentModel.Save(new TrimesterCommentModel()
            {
                DivisionPrefectComment = DivisionPrefectCommentTextBox.Text,
                MainTeacherComment = MainTeacherCommentTextBox.Text,
                Trimester = entryTrimesterComments.SelectedTrimester,
                IdStudent = entryTrimesterComments.SelectedClass.SelectedStudent.Student.Id,
                Year = entryTrimesterComments.SelectedClass.Class.Year,
            });
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTeacherCommentTextBox != null)
            {
                MainTeacherCommentTextBox.Focus();
            }
        }
    }
}
