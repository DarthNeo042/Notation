using System.Windows;

namespace Notation.ViewModels
{
    public class PeriodCommentViewModel : BaseViewModel
    {
        public enum ReportEnum
        {
            Good = 0,
            MustProgress = 1,
            Insufficient = 2,
            Warning = 3,
        }

        public int IdPeriod
        {
            get { return (int)GetValue(IdPeriodProperty); }
            set { SetValue(IdPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdPeriodProperty =
            DependencyProperty.Register("IdPeriod", typeof(int), typeof(PeriodCommentViewModel), new PropertyMetadata(0));

        public int IdStudent
        {
            get { return (int)GetValue(IdStudentProperty); }
            set { SetValue(IdStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdStudentProperty =
            DependencyProperty.Register("IdStudent", typeof(int), typeof(PeriodCommentViewModel), new PropertyMetadata(0));

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
