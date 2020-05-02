﻿using System.Windows;

namespace Notation.ViewModels
{
    public class SemiTrimesterCommentViewModel : BaseViewModel
    {
        public string MainTeacherComment
        {
            get { return (string)GetValue(MainTeacherCommentProperty); }
            set { SetValue(MainTeacherCommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainTeacherComment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTeacherCommentProperty =
            DependencyProperty.Register("MainTeacherComment", typeof(string), typeof(SemiTrimesterCommentViewModel), new PropertyMetadata(""));

        public string DivisionPrefectComment
        {
            get { return (string)GetValue(DivisionPrefectCommentProperty); }
            set { SetValue(DivisionPrefectCommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DivisionPrefectComment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DivisionPrefectCommentProperty =
            DependencyProperty.Register("DivisionPrefectComment", typeof(string), typeof(SemiTrimesterCommentViewModel), new PropertyMetadata(""));

        public StudentViewModel Student
        {
            get { return (StudentViewModel)GetValue(StudentProperty); }
            set { SetValue(StudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Student.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentProperty =
            DependencyProperty.Register("Student", typeof(StudentViewModel), typeof(SemiTrimesterViewModel), new PropertyMetadata(null));

        public SemiTrimesterViewModel SemiTrimester
        {
            get { return (SemiTrimesterViewModel)GetValue(SemiTrimesterProperty); }
            set { SetValue(SemiTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SemiTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SemiTrimesterProperty =
            DependencyProperty.Register("SemiTrimester", typeof(SemiTrimesterViewModel), typeof(SemiTrimesterViewModel), new PropertyMetadata(null));
    }
}
