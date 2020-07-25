using Notation.Models;
using Notation.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour InputSemiTrimesterComments.xaml
    /// </summary>
    public partial class EntrySemiTrimesterComments : Window
    {
        public EntrySemiTrimesterComments()
        {
            EntrySemiTrimesterCommentsViewModel entrySemiTrimesterComments = new EntrySemiTrimesterCommentsViewModel();
            DataContext = entrySemiTrimesterComments;
            InitializeComponent();

            entrySemiTrimesterComments.SelectedClassChangedEvent += EntrySemiTrimesterComments_SelectedClassChangedEvent;
            EntrySemiTrimesterComments_SelectedClassChangedEvent();
            SelectedClass_SelectedStudentChangedEvent();

            ListView_SelectionChanged(null, null);
        }

        private void EntrySemiTrimesterComments_SelectedClassChangedEvent()
        {
            EntrySemiTrimesterCommentsViewModel entrySemiTrimesterComments = (EntrySemiTrimesterCommentsViewModel)DataContext;
            if (entrySemiTrimesterComments.SelectedClass != null && !entrySemiTrimesterComments.SelectedClass.SelectedStudentChangedSet)
            {
                entrySemiTrimesterComments.SelectedClass.SelectedStudentChangedEvent += SelectedClass_SelectedStudentChangedEvent;
                entrySemiTrimesterComments.SelectedClass.SelectedStudent = entrySemiTrimesterComments.SelectedClass.Students.FirstOrDefault();
            }
            SelectedClass_SelectedStudentChangedEvent();
        }

        private void SelectedClass_SelectedStudentChangedEvent()
        {
            EntrySemiTrimesterCommentsViewModel entrySemiTrimesterComments = (EntrySemiTrimesterCommentsViewModel)DataContext;

            SemiTrimesterCommentViewModel semiTrimesterComment = SemiTrimesterCommentModel.Read(entrySemiTrimesterComments.SelectedSemiTrimester, entrySemiTrimesterComments.SelectedClass.SelectedStudent.Student);
            if (semiTrimesterComment != null)
            {
                MainTeacherCommentTextBox.Text = semiTrimesterComment.MainTeacherComment;
                DivisionPrefectCommentTextBox.Text = semiTrimesterComment.DivisionPrefectComment;
            }
            else
            {
                MainTeacherCommentTextBox.Text = "";
                DivisionPrefectCommentTextBox.Text = "";
            }
        }

        private void SemiTrimesterComment_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveSemiTrimesterComments((EntrySemiTrimesterCommentsViewModel)DataContext);
        }

        private void SemiTrimesterComment_KeyDown(object sender, KeyEventArgs e)
        {
            EntrySemiTrimesterCommentsViewModel entrySemiTrimesterComments = (EntrySemiTrimesterCommentsViewModel)DataContext;

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
                            SaveSemiTrimesterComments(entrySemiTrimesterComments);
                            if (entrySemiTrimesterComments.SelectedClass.SelectedStudent != entrySemiTrimesterComments.SelectedClass.Students.Last())
                            {
                                entrySemiTrimesterComments.SelectedClass.SelectedStudent
                                    = entrySemiTrimesterComments.SelectedClass.Students[entrySemiTrimesterComments.SelectedClass.Students.IndexOf(entrySemiTrimesterComments.SelectedClass.SelectedStudent) + 1];
                            }
                            else
                            {
                                if (entrySemiTrimesterComments.SelectedClass != entrySemiTrimesterComments.Classes.Last())
                                {
                                    entrySemiTrimesterComments.SelectedClass = entrySemiTrimesterComments.Classes[entrySemiTrimesterComments.Classes.IndexOf(entrySemiTrimesterComments.SelectedClass) + 1];
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
                            SaveSemiTrimesterComments(entrySemiTrimesterComments);
                            if (entrySemiTrimesterComments.SelectedClass.SelectedStudent != entrySemiTrimesterComments.SelectedClass.Students.First())
                            {
                                entrySemiTrimesterComments.SelectedClass.SelectedStudent
                                    = entrySemiTrimesterComments.SelectedClass.Students[entrySemiTrimesterComments.SelectedClass.Students.IndexOf(entrySemiTrimesterComments.SelectedClass.SelectedStudent) - 1];
                            }
                            else
                            {
                                if (entrySemiTrimesterComments.SelectedClass != entrySemiTrimesterComments.Classes.First())
                                {
                                    entrySemiTrimesterComments.SelectedClass = entrySemiTrimesterComments.Classes[entrySemiTrimesterComments.Classes.IndexOf(entrySemiTrimesterComments.SelectedClass) - 1];
                                    entrySemiTrimesterComments.SelectedClass.SelectedStudent = entrySemiTrimesterComments.SelectedClass.Students.FirstOrDefault();
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void SaveSemiTrimesterComments(EntrySemiTrimesterCommentsViewModel entrySemiTrimesterComments)
        {
            SemiTrimesterCommentModel.Save(new SemiTrimesterCommentViewModel()
            {
                DivisionPrefectComment = DivisionPrefectCommentTextBox.Text,
                MainTeacherComment = MainTeacherCommentTextBox.Text,
                SemiTrimester = entrySemiTrimesterComments.SelectedSemiTrimester,
                Student = entrySemiTrimesterComments.SelectedClass.SelectedStudent.Student,
                Year = entrySemiTrimesterComments.SelectedSemiTrimester.Year,
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
