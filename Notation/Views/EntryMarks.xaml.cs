using Notation.Models;
using Notation.ViewModels;
using System.Collections.Generic;
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
            Marks1.Children.Clear();
            Marks2.Children.Clear();
            Marks4.Children.Clear();
            if (!((EntryMarksViewModel)DataContext).SelectedClass.SelectedStudentChangedSet)
            {
                ((EntryMarksViewModel)DataContext).SelectedClass.SelectedStudentChangedEvent += SelectedClass_SelectedStudentChangedEvent;
            }
        }

        private void SelectedClass_SelectedStudentChangedEvent()
        {
            Marks1.Children.Clear();
            Marks2.Children.Clear();
            Marks4.Children.Clear();
            if (!((EntryMarksViewModel)DataContext).SelectedClass.SelectedStudent.SelectedSubjectChangedSet)
            {
                ((EntryMarksViewModel)DataContext).SelectedClass.SelectedStudent.SelectedSubjectChangedEvent += SelectedStudent_SelectedSubjectChangedEvent;
            }
        }

        private void SelectedStudent_SelectedSubjectChangedEvent()
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)DataContext;

            Marks1.Children.Clear();
            Marks2.Children.Clear();
            Marks4.Children.Clear();
            TextBox textBox = null;
            if (entryMarks.SelectedTeacher != null)
            {
                foreach (MarkViewModel mark in MarkModel.Read(new List<int>() { entryMarks.SelectedClass.SelectedStudent.SelectedMarksSubject.Subject.Id },
                    entryMarks.SelectedClass.SelectedStudent.Student.Id, entryMarks.SelectedTeacher.Id, entryMarks.SelectedClass.Class.Id, entryMarks.SelectedPeriod.Id, entryMarks.SelectedPeriod.Year))
                {
                    textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = mark.Mark.ToString() };
                    textBox.PreviewKeyDown += Mark_KeyDown;
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
            Marks1.Children.Add(textBox);
            textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
            textBox.PreviewKeyDown += Mark_KeyDown;
            textBox.TextChanged += Mark_TextChanged;
            Marks2.Children.Add(textBox);
            textBox = new TextBox() { FontSize = 25, Margin = new Thickness(30, 0, 0, 0), Width = 40, Text = "" };
            textBox.PreviewKeyDown += Mark_KeyDown;
            textBox.TextChanged += Mark_TextChanged;
            Marks4.Children.Add(textBox);
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
                            stackPanel.Children.Add(textBox2);
                            textBox2.Focus();
                        }
                        e.Handled = true;
                    }
                    break;
            }
        }
    }
}
