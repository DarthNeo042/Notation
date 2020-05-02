using System.Windows;

namespace Notation.ViewModels
{
    public class PeriodCommentViewModel : BaseViewModel
    {
        public enum ReportEnum
        {
            Good = 1,
            MustProgress = 2,
            Insufficient = 3,
            Warning = 4,
        }

        public PeriodViewModel Period
        {
            get { return (PeriodViewModel)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Period.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", typeof(PeriodViewModel), typeof(PeriodCommentViewModel), new PropertyMetadata(null));

        public StudentViewModel Student
        {
            get { return (StudentViewModel)GetValue(StudentProperty); }
            set { SetValue(StudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Student.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentProperty =
            DependencyProperty.Register("Student", typeof(StudentViewModel), typeof(PeriodCommentViewModel), new PropertyMetadata(null));

        public ReportEnum StudiesReport
        {
            get { return (ReportEnum)GetValue(StudiesReportProperty); }
            set { SetValue(StudiesReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StudiesReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudiesReportProperty =
            DependencyProperty.Register("StudiesReport", typeof(ReportEnum), typeof(PeriodCommentViewModel), new PropertyMetadata(ReportEnum.Good));

        public ReportEnum DisciplineReport
        {
            get { return (ReportEnum)GetValue(DisciplineReportProperty); }
            set { SetValue(DisciplineReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisciplineReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisciplineReportProperty =
            DependencyProperty.Register("DisciplineReport", typeof(ReportEnum), typeof(PeriodCommentViewModel), new PropertyMetadata(ReportEnum.Good));
    }
}
