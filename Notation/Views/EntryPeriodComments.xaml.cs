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
    /// Logique d'interaction pour InputPeriodComments.xaml
    /// </summary>
    public partial class EntryPeriodComments : Window
    {
        public EntryPeriodComments()
        {
            EntryPeriodCommentsViewModel entryPeriodComments = new EntryPeriodCommentsViewModel();
            DataContext = entryPeriodComments;
            InitializeComponent();

            entryPeriodComments.SelectedClassChangedEvent += EntryPeriodComments_SelectedClassChangedEvent;
            EntryPeriodComments_SelectedClassChangedEvent();
            SelectedClass_SelectedStudentChangedEvent();

            ListView_SelectionChanged(null, null);
        }

        private void EntryPeriodComments_SelectedClassChangedEvent()
        {
            EntryPeriodCommentsViewModel entryPeriodComments = (EntryPeriodCommentsViewModel)DataContext;
            if (entryPeriodComments.SelectedClass != null && !entryPeriodComments.SelectedClass.SelectedStudentChangedSet)
            {
                entryPeriodComments.SelectedClass.SelectedStudentChangedEvent += SelectedClass_SelectedStudentChangedEvent;
                entryPeriodComments.SelectedClass.SelectedStudent = entryPeriodComments.SelectedClass.Students.FirstOrDefault();
            }
            SelectedClass_SelectedStudentChangedEvent();
        }

        private void SelectedClass_SelectedStudentChangedEvent()
        {
            EntryPeriodCommentsViewModel entryPeriodComments = (EntryPeriodCommentsViewModel)DataContext;

            PeriodCommentModel periodComment = PeriodCommentModel.Read(entryPeriodComments.SelectedPeriod, entryPeriodComments.SelectedClass.SelectedStudent.Student);
            if (periodComment != null)
            {
                switch (periodComment.DisciplineReport)
                {
                    case 1:
                        Discipline1Radio.IsChecked = true;
                        break;
                    case 2:
                        Discipline2Radio.IsChecked = true;
                        break;
                    case 3:
                        Discipline3Radio.IsChecked = true;
                        break;
                    case 4:
                        DisciplineARadio.IsChecked = true;
                        break;
                }
                switch (periodComment.StudiesReport)
                {
                    case 1:
                        Studies1Radio.IsChecked = true;
                        break;
                    case 2:
                        Studies2Radio.IsChecked = true;
                        break;
                    case 3:
                        Studies3Radio.IsChecked = true;
                        break;
                    case 4:
                        StudiesARadio.IsChecked = true;
                        break;
                }
            }
            else
            {
                Studies1Radio.IsChecked = true;
                Discipline1Radio.IsChecked = true;
            }

            StudiesTextBox.Text = "";
            DisciplineTextBox.Text = "";
        }

        private void PeriodComment_LostFocus(object sender, RoutedEventArgs e)
        {
            SavePeriodComments((EntryPeriodCommentsViewModel)DataContext);
        }

        private void PeriodComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text != "1" && textBox.Text != "2" && textBox.Text != "3" && textBox.Text != "A" && textBox.Text != "a")
            {
                textBox.Text = "";
            }
            switch (DisciplineTextBox.Text.ToUpper())
            {
                case "1":
                    Discipline1Radio.IsChecked = true;
                    break;
                case "2":
                    Discipline2Radio.IsChecked = true;
                    break;
                case "3":
                    Discipline3Radio.IsChecked = true;
                    break;
                case "A":
                    DisciplineARadio.IsChecked = true;
                    break;
            }
            switch (StudiesTextBox.Text.ToUpper())
            {
                case "1":
                    Studies1Radio.IsChecked = true;
                    break;
                case "2":
                    Studies2Radio.IsChecked = true;
                    break;
                case "3":
                    Studies3Radio.IsChecked = true;
                    break;
                case "A":
                    StudiesARadio.IsChecked = true;
                    break;
            }
        }

        private void PeriodComment_KeyDown(object sender, KeyEventArgs e)
        {
            EntryPeriodCommentsViewModel entryPeriodComments = (EntryPeriodCommentsViewModel)DataContext;

            switch (e.Key)
            {
                case Key.Down:
                    {
                        TextBox textBox = (TextBox)sender;
                        if (textBox == StudiesTextBox)
                        {
                            DisciplineTextBox.Focus();
                        }
                        else if (textBox == DisciplineTextBox)
                        {
                            SavePeriodComments(entryPeriodComments);
                            if (entryPeriodComments.SelectedClass.SelectedStudent != entryPeriodComments.SelectedClass.Students.Last())
                            {
                                entryPeriodComments.SelectedClass.SelectedStudent
                                    = entryPeriodComments.SelectedClass.Students[entryPeriodComments.SelectedClass.Students.IndexOf(entryPeriodComments.SelectedClass.SelectedStudent) + 1];
                            }
                            else
                            {
                                if (entryPeriodComments.SelectedClass != entryPeriodComments.Classes.Last())
                                {
                                    entryPeriodComments.SelectedClass = entryPeriodComments.Classes[entryPeriodComments.Classes.IndexOf(entryPeriodComments.SelectedClass) + 1];
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
                        if (textBox == DisciplineTextBox)
                        {
                            StudiesTextBox.Focus();
                        }
                        else if (textBox == StudiesTextBox)
                        {
                            SavePeriodComments(entryPeriodComments);
                            if (entryPeriodComments.SelectedClass.SelectedStudent != entryPeriodComments.SelectedClass.Students.First())
                            {
                                entryPeriodComments.SelectedClass.SelectedStudent
                                    = entryPeriodComments.SelectedClass.Students[entryPeriodComments.SelectedClass.Students.IndexOf(entryPeriodComments.SelectedClass.SelectedStudent) - 1];
                            }
                            else
                            {
                                if (entryPeriodComments.SelectedClass != entryPeriodComments.Classes.First())
                                {
                                    entryPeriodComments.SelectedClass = entryPeriodComments.Classes[entryPeriodComments.Classes.IndexOf(entryPeriodComments.SelectedClass) - 1];
                                    entryPeriodComments.SelectedClass.SelectedStudent = entryPeriodComments.SelectedClass.Students.FirstOrDefault();
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void SavePeriodComments(EntryPeriodCommentsViewModel entryPeriodComments)
        {
            PeriodCommentModel periodComment = new PeriodCommentModel()
            {
                IdPeriod = entryPeriodComments.SelectedPeriod.Id,
                IdStudent = entryPeriodComments.SelectedClass.SelectedStudent.Student.Id,
                Year = entryPeriodComments.SelectedPeriod.Year,
            };

            if (Studies1Radio.IsChecked ?? false)
            {
                periodComment.StudiesReport = 1;
            }
            else if (Studies2Radio.IsChecked ?? false)
            {
                periodComment.StudiesReport = 2;
            }
            else if (Studies3Radio.IsChecked ?? false)
            {
                periodComment.StudiesReport = 3;
            }
            else if (StudiesARadio.IsChecked ?? false)
            {
                periodComment.StudiesReport = 4;
            }
            if (Discipline1Radio.IsChecked ?? false)
            {
                periodComment.DisciplineReport = 1;
            }
            else if (Discipline2Radio.IsChecked ?? false)
            {
                periodComment.DisciplineReport = 2;
            }
            else if (Discipline3Radio.IsChecked ?? false)
            {
                periodComment.DisciplineReport = 3;
            }
            else if (DisciplineARadio.IsChecked ?? false)
            {
                periodComment.DisciplineReport = 4;
            }

            PeriodCommentModel.Save(new List<PeriodCommentModel>() { periodComment }, entryPeriodComments.SelectedPeriod.Year);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudiesTextBox != null)
            {
                StudiesTextBox.Focus();
            }
        }
    }
}
