using Notation.Models;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour InputTrimesterSubjectComments.xaml
    /// </summary>
    public partial class EntryTrimesterSubjectComments : Window
    {
        public EntryTrimesterSubjectComments()
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = new EntryTrimesterSubjectCommentsViewModel();
            DataContext = entryTrimesterSubjectComments;
            InitializeComponent();

            entryTrimesterSubjectComments.SelectedClassChangedEvent += EntryTrimesterSubjectComments_SelectedClassChangedEvent;
            EntryTrimesterSubjectComments_SelectedClassChangedEvent();
            SelectedClass_SelectedStudentChangedEvent();
            SelectedStudent_SelectedSubjectChangedEvent();

            Teacher.IsEnabled = MainViewModel.Instance.User.IsAdmin;
        }

        private void EntryTrimesterSubjectComments_SelectedClassChangedEvent()
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = (EntryTrimesterSubjectCommentsViewModel)DataContext;
            if (entryTrimesterSubjectComments.SelectedClass != null && !entryTrimesterSubjectComments.SelectedClass.SelectedStudentChangedSet)
            {
                entryTrimesterSubjectComments.SelectedClass.SelectedStudentChangedEvent += SelectedClass_SelectedStudentChangedEvent;
                entryTrimesterSubjectComments.SelectedClass.SelectedStudent = entryTrimesterSubjectComments.SelectedClass.Students.FirstOrDefault();
            }
            SelectedClass_SelectedStudentChangedEvent();
        }

        private void SelectedClass_SelectedStudentChangedEvent()
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = (EntryTrimesterSubjectCommentsViewModel)DataContext;
            if (entryTrimesterSubjectComments.SelectedClass != null && entryTrimesterSubjectComments.SelectedClass.SelectedStudent != null && !entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedSubjectChangedSet)
            {
                entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedSubjectChangedEvent += SelectedStudent_SelectedSubjectChangedEvent;
                entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.FirstOrDefault();
            }
            SelectedStudent_SelectedSubjectChangedEvent();
        }

        private void SelectedStudent_SelectedSubjectChangedEvent()
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = (EntryTrimesterSubjectCommentsViewModel)DataContext;

            if (entryTrimesterSubjectComments.SelectedClass != null)
            {
                TrimesterSubjectCommentViewModel trimesterSubjectComment = TrimesterSubjectCommentModel.Read(entryTrimesterSubjectComments.SelectedTrimester,
                entryTrimesterSubjectComments.SelectedClass.SelectedStudent.Student, entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject.Subject);
                if (trimesterSubjectComment != null)
                {
                    CommentTextBox.Text = trimesterSubjectComment.Comment;
                }
                else
                {
                    CommentTextBox.Text = "";
                }
            }
        }

        private void TrimesterSubjectComment_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveTrimesterSubjectComments((EntryTrimesterSubjectCommentsViewModel)DataContext);
        }

        private void TrimesterSubjectComment_KeyDown(object sender, KeyEventArgs e)
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = (EntryTrimesterSubjectCommentsViewModel)DataContext;

            switch (e.Key)
            {
                case Key.Down:
                    {
                        SaveTrimesterSubjectComments(entryTrimesterSubjectComments);
                        if (entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject != entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.Last())
                        {
                            entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject
                                = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects[entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.IndexOf(entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject) + 1];
                        }
                        else
                        {
                            if (entryTrimesterSubjectComments.SelectedClass.SelectedStudent != entryTrimesterSubjectComments.SelectedClass.Students.Last())
                            {
                                entryTrimesterSubjectComments.SelectedClass.SelectedStudent
                                    = entryTrimesterSubjectComments.SelectedClass.Students[entryTrimesterSubjectComments.SelectedClass.Students.IndexOf(entryTrimesterSubjectComments.SelectedClass.SelectedStudent) + 1];
                                entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects .FirstOrDefault();
                            }
                            else
                            {
                                if (entryTrimesterSubjectComments.SelectedClass != entryTrimesterSubjectComments.Classes.Last())
                                {
                                    entryTrimesterSubjectComments.SelectedClass = entryTrimesterSubjectComments.Classes[entryTrimesterSubjectComments.Classes.IndexOf(entryTrimesterSubjectComments.SelectedClass) + 1];
                                    entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.FirstOrDefault();
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
                        SaveTrimesterSubjectComments(entryTrimesterSubjectComments);
                        if (entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject != entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.First())
                        {
                            entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject
                                = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects[entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.IndexOf(entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject) - 1];
                        }
                        else
                        {
                            if (entryTrimesterSubjectComments.SelectedClass.SelectedStudent != entryTrimesterSubjectComments.SelectedClass.Students.First())
                            {
                                entryTrimesterSubjectComments.SelectedClass.SelectedStudent
                                    = entryTrimesterSubjectComments.SelectedClass.Students[entryTrimesterSubjectComments.SelectedClass.Students.IndexOf(entryTrimesterSubjectComments.SelectedClass.SelectedStudent) - 1];
                                entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.FirstOrDefault();
                            }
                            else
                            {
                                if (entryTrimesterSubjectComments.SelectedClass != entryTrimesterSubjectComments.Classes.First())
                                {
                                    entryTrimesterSubjectComments.SelectedClass = entryTrimesterSubjectComments.Classes[entryTrimesterSubjectComments.Classes.IndexOf(entryTrimesterSubjectComments.SelectedClass) - 1];
                                    entryTrimesterSubjectComments.SelectedClass.SelectedStudent = entryTrimesterSubjectComments.SelectedClass.Students.FirstOrDefault();
                                    entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.FirstOrDefault();
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void SaveTrimesterSubjectComments(EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments)
        {
            TrimesterSubjectCommentModel.Save(new TrimesterSubjectCommentViewModel()
            {
                Comment = CommentTextBox.Text,
                Trimester = entryTrimesterSubjectComments.SelectedTrimester,
                Student = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.Student,
                Subject = entryTrimesterSubjectComments.SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject.Subject,
                Year = entryTrimesterSubjectComments.SelectedTeacher.Year,
            });
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CommentTextBox != null)
            {
                CommentTextBox.Focus();
            }
        }
    }
}
