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
    /// Logique d'interaction pour InputMarks.xaml
    /// </summary>
    public partial class EntryMarks : Window
    {
        public EntryMarks()
        {
            EntryMarksViewModel entryMarks = new EntryMarksViewModel();
            DataContext = entryMarks;
            InitializeComponent();

            entryMarks.SelectedClassChangedEvent += EntryMarks_SelectedClassChangedEvent;
            EntryMarks_SelectedClassChangedEvent();
            SelectedClass_SelectedStudentChangedEvent();
            SelectedStudent_SelectedSubjectChangedEvent();

            Teacher.IsEnabled = MainViewModel.Instance.User.IsAdmin;
        }

        private void EntryMarks_SelectedClassChangedEvent()
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)DataContext;
            if (entryMarks.SelectedClass != null && !entryMarks.SelectedClass.SelectedStudentChangedSet)
            {
                entryMarks.SelectedClass.SelectedStudentChangedEvent += SelectedClass_SelectedStudentChangedEvent;
                entryMarks.SelectedClass.SelectedStudent = entryMarks.SelectedClass.Students.FirstOrDefault();
            }
            SelectedClass_SelectedStudentChangedEvent();
        }

        private void SelectedClass_SelectedStudentChangedEvent()
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)DataContext;
            if (entryMarks.SelectedClass != null && entryMarks.SelectedClass.SelectedStudent != null && !entryMarks.SelectedClass.SelectedStudent.SelectedSubjectChangedSet)
            {
                entryMarks.SelectedClass.SelectedStudent.SelectedSubjectChangedEvent += SelectedStudent_SelectedSubjectChangedEvent;
                entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject = entryMarks.SelectedClass.SelectedStudent.MarksSubjects.FirstOrDefault();
            }
            SelectedStudent_SelectedSubjectChangedEvent();
            CalculateAverages((EntryMarksViewModel)DataContext);
        }

        private void SelectedStudent_SelectedSubjectChangedEvent()
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)DataContext;

            Marks1.Children.Clear();
            Marks2.Children.Clear();
            Marks4.Children.Clear();
            if (entryMarks.SelectedClass != null)
            {
                TextBox textBox = null;
                if (entryMarks.SelectedTeacher != null && entryMarks.SelectedClass.SelectedStudent != null && entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject != null)
                {
                    foreach (MarkViewModel mark in MarkModel.Read(new List<int>() { entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject.Subject.Id },
                        entryMarks.SelectedClass.SelectedStudent.Student.Id, entryMarks.SelectedTeacher.Id, entryMarks.SelectedClass.Class.Id, entryMarks.SelectedPeriod.Id, entryMarks.SelectedPeriod.Year))
                    {
                        textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = mark.Mark.ToString() };
                        textBox.PreviewKeyDown += Mark_KeyDown;
                        textBox.TextChanged += Mark_TextChanged;
                        textBox.LostFocus += Mark_LostFocus;
                        switch (mark.Coefficient)
                        {
                            case 1:
                                Marks1.Children.Add(textBox);
                                break;
                            case 2:
                                Marks2.Children.Add(textBox);
                                break;
                            case 4:
                                Marks4.Children.Add(textBox);
                                break;
                        }
                    }
                }
                textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
                textBox.PreviewKeyDown += Mark_KeyDown;
                textBox.TextChanged += Mark_TextChanged;
                textBox.LostFocus += Mark_LostFocus;
                Marks1.Children.Add(textBox);
                textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
                textBox.PreviewKeyDown += Mark_KeyDown;
                textBox.TextChanged += Mark_TextChanged;
                textBox.LostFocus += Mark_LostFocus;
                Marks2.Children.Add(textBox);
                textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
                textBox.PreviewKeyDown += Mark_KeyDown;
                textBox.TextChanged += Mark_TextChanged;
                textBox.LostFocus += Mark_LostFocus;
                Marks4.Children.Add(textBox);
            }
        }

        private void Mark_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveMarks((EntryMarksViewModel)DataContext);
            CalculateAverages((EntryMarksViewModel)DataContext);
        }

        private void Mark_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (int.TryParse(textBox.Text, out int mark))
            {
                if (mark > 20)
                {
                    textBox.Text = "20";
                }
                else if (mark < 0)
                {
                    textBox.Text = "0";
                }

            }
            else
            {
                textBox.Text = "";
            }
        }

        private void Mark_KeyDown(object sender, KeyEventArgs e)
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)DataContext;

            switch (e.Key)
            {
                case Key.Right:
                    {
                        TextBox textBox = (TextBox)sender;
                        StackPanel stackPanel = (StackPanel)textBox.Parent;
                        if (stackPanel.Children.Count > stackPanel.Children.IndexOf(textBox) + 1)
                        {
                            TextBox textBox2 = (TextBox)stackPanel.Children[stackPanel.Children.IndexOf(textBox) + 1];
                            textBox2.Focus();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(textBox.Text))
                            {
                                TextBox textBox2 = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
                                textBox2.PreviewKeyDown += Mark_KeyDown;
                                textBox2.TextChanged += Mark_TextChanged;
                                textBox2.LostFocus += Mark_LostFocus;
                                stackPanel.Children.Add(textBox2);
                                textBox2.Focus();
                            }
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Left:
                    {
                        TextBox textBox = (TextBox)sender;
                        StackPanel stackPanel = (StackPanel)textBox.Parent;
                        if (stackPanel.Children.IndexOf(textBox) > 0)
                        {
                            TextBox textBox2 = (TextBox)stackPanel.Children[stackPanel.Children.IndexOf(textBox) - 1];
                            textBox2.Focus();
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    {
                        TextBox textBox = (TextBox)sender;
                        StackPanel stackPanel = (StackPanel)textBox.Parent;
                        if (stackPanel == Marks1)
                        {
                            Marks2.Children[0].Focus();
                        }
                        else if (stackPanel == Marks2)
                        {
                            Marks4.Children[0].Focus();
                        }
                        else if (stackPanel == Marks4)
                        {
                            SaveMarks((EntryMarksViewModel)DataContext);
                            CalculateAverages((EntryMarksViewModel)DataContext);
                            if (entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject != entryMarks.SelectedClass.SelectedStudent.MarksSubjects.Last())
                            {
                                entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject
                                    = entryMarks.SelectedClass.SelectedStudent.MarksSubjects[entryMarks.SelectedClass.SelectedStudent.MarksSubjects.IndexOf(entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject) + 1];
                            }
                            else
                            {
                                if (entryMarks.SelectedClass.SelectedStudent != entryMarks.SelectedClass.Students.Last())
                                {
                                    entryMarks.SelectedClass.SelectedStudent
                                        = entryMarks.SelectedClass.Students[entryMarks.SelectedClass.Students.IndexOf(entryMarks.SelectedClass.SelectedStudent) + 1];
                                }
                                else
                                {
                                    if (entryMarks.SelectedClass != entryMarks.Classes.Last())
                                    {
                                        entryMarks.SelectedClass = entryMarks.Classes[entryMarks.Classes.IndexOf(entryMarks.SelectedClass) + 1];
                                    }
                                    else
                                    {
                                        MessageBox.Show("Fin de la saisie.", "Fin", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Up:
                    {
                        TextBox textBox = (TextBox)sender;
                        StackPanel stackPanel = (StackPanel)textBox.Parent;
                        if (stackPanel == Marks4)
                        {
                            Marks2.Children[0].Focus();
                        }
                        else if (stackPanel == Marks2)
                        {
                            Marks1.Children[0].Focus();
                        }
                        else if (stackPanel == Marks1)
                        {
                            SaveMarks((EntryMarksViewModel)DataContext);
                            CalculateAverages((EntryMarksViewModel)DataContext);
                            if (entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject != entryMarks.SelectedClass.SelectedStudent.MarksSubjects.First())
                            {
                                entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject
                                    = entryMarks.SelectedClass.SelectedStudent.MarksSubjects[entryMarks.SelectedClass.SelectedStudent.MarksSubjects.IndexOf(entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject) - 1];
                            }
                            else
                            {
                                if (entryMarks.SelectedClass.SelectedStudent != entryMarks.SelectedClass.Students.First())
                                {
                                    entryMarks.SelectedClass.SelectedStudent
                                        = entryMarks.SelectedClass.Students[entryMarks.SelectedClass.Students.IndexOf(entryMarks.SelectedClass.SelectedStudent) - 1];
                                    entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject = entryMarks.SelectedClass.SelectedStudent.MarksSubjects.FirstOrDefault();
                                }
                                else
                                {
                                    if (entryMarks.SelectedClass != entryMarks.Classes.First())
                                    {
                                        entryMarks.SelectedClass = entryMarks.Classes[entryMarks.Classes.IndexOf(entryMarks.SelectedClass) - 1];
                                        entryMarks.SelectedClass.SelectedStudent = entryMarks.SelectedClass.Students.FirstOrDefault();
                                        entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject = entryMarks.SelectedClass.SelectedStudent.MarksSubjects.FirstOrDefault();
                                    }
                                }
                            }
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Delete:
                    {
                        TextBox textBox = (TextBox)sender;
                        StackPanel stackPanel = (StackPanel)textBox.Parent;
                        int index = stackPanel.Children.IndexOf(textBox);
                        stackPanel.Children.Remove(textBox);
                        if (stackPanel.Children.Count > 0)
                        {
                            TextBox textBox2 = (TextBox)stackPanel.Children[index > 0 ? index - 1 : 0];
                            textBox2.Focus();
                        }
                        else
                        {
                            TextBox textBox2 = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
                            textBox2.PreviewKeyDown += Mark_KeyDown;
                            textBox2.TextChanged += Mark_TextChanged;
                            textBox2.LostFocus += Mark_LostFocus;
                            stackPanel.Children.Add(textBox2);
                            textBox2.Focus();
                        }
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void SaveMarks(EntryMarksViewModel entryMarks)
        {
            List<MarkViewModel> marks = new List<MarkViewModel>();
            foreach (TextBox textBox in Marks1.Children)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    marks.Add(new MarkViewModel()
                    {
                        Coefficient = 1,
                        IdClass = entryMarks.SelectedClass.Class.Id,
                        IdPeriod = entryMarks.SelectedPeriod.Id,
                        IdStudent = entryMarks.SelectedClass.SelectedStudent.Student.Id,
                        IdSubject = entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject.Subject.Id,
                        IdTeacher = entryMarks.SelectedTeacher.Id,
                        Mark = int.Parse(textBox.Text),
                        Year = entryMarks.SelectedPeriod.Year,
                    });
                }
            }
            foreach (TextBox textBox in Marks2.Children)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    marks.Add(new MarkViewModel()
                    {
                        Coefficient = 2,
                        IdClass = entryMarks.SelectedClass.Class.Id,
                        IdPeriod = entryMarks.SelectedPeriod.Id,
                        IdStudent = entryMarks.SelectedClass.SelectedStudent.Student.Id,
                        IdSubject = entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject.Subject.Id,
                        IdTeacher = entryMarks.SelectedTeacher.Id,
                        Mark = int.Parse(textBox.Text),
                        Year = entryMarks.SelectedPeriod.Year,
                    });
                }
            }
            foreach (TextBox textBox in Marks4.Children)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    marks.Add(new MarkViewModel()
                    {
                        Coefficient = 4,
                        IdClass = entryMarks.SelectedClass.Class.Id,
                        IdPeriod = entryMarks.SelectedPeriod.Id,
                        IdStudent = entryMarks.SelectedClass.SelectedStudent.Student.Id,
                        IdSubject = entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject.Subject.Id,
                        IdTeacher = entryMarks.SelectedTeacher.Id,
                        Mark = int.Parse(textBox.Text),
                        Year = entryMarks.SelectedPeriod.Year,
                    });
                }
            }
            MarkModel.Save(marks, entryMarks.SelectedPeriod.Year);
        }

        private void CalculateAverages(EntryMarksViewModel entryMarks)
        {
            if (entryMarks.SelectedClass != null && entryMarks.SelectedClass.SelectedStudent != null)
            {
                foreach (EntryMarksSubjectViewModel marksSubject in entryMarks.SelectedClass.SelectedStudent.MarksSubjects)
                {
                    double average = double.MinValue;
                    if (marksSubject.Subject.ChildrenSubjects.Any())
                    {
                        average = MarkModel.ReadPeriodMainSubjectAverage(entryMarks.SelectedPeriod, entryMarks.SelectedClass.SelectedStudent.Student, marksSubject.Subject);
                    }
                    else
                    {
                        average = MarkModel.ReadPeriodSubjectAverage(entryMarks.SelectedPeriod, entryMarks.SelectedClass.SelectedStudent.Student, marksSubject.Subject);
                    }
                    marksSubject.Average = average != double.MinValue ? average.ToString("0.0") : "";
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Marks1 != null && Marks1.Children.Count > 0)
            {
                Marks1.Children[0].Focus();
            }
        }
    }
}
