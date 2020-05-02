using Notation.Utils;
using System;
using System.Windows;

namespace Notation.ViewModels
{
    public class SemiTrimesterViewModel : BaseViewModel
    {
        public PeriodViewModel Period1
        {
            get { return (PeriodViewModel)GetValue(Period1Property); }
            set { SetValue(Period1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Period1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Period1Property =
            DependencyProperty.Register("Period1", typeof(PeriodViewModel), typeof(SemiTrimesterViewModel), new PropertyMetadata(null, PeriodChanged));

        private static void PeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SemiTrimesterViewModel semiTrimester = (SemiTrimesterViewModel)d;
            if (semiTrimester.Period1 != null)
            {
                semiTrimester.FromDate = semiTrimester.Period1.FromDate;
                if (semiTrimester.Period2 != null)
                {
                    semiTrimester.ToDate = semiTrimester.Period2.ToDate;
                }
                else
                {
                    semiTrimester.ToDate = semiTrimester.Period1.ToDate;
                }
                if (string.IsNullOrEmpty(semiTrimester.Name))
                {
                    semiTrimester.Name = MonthUtils.Name(semiTrimester.ToDate.Month);
                }
            }
        }

        public PeriodViewModel Period2
        {
            get { return (PeriodViewModel)GetValue(Period2Property); }
            set { SetValue(Period2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Period2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Period2Property =
            DependencyProperty.Register("Period2", typeof(PeriodViewModel), typeof(SemiTrimesterViewModel), new PropertyMetadata(null, PeriodChanged));

        public DateTime FromDate
        {
            get { return (DateTime)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime), typeof(SemiTrimesterViewModel), new PropertyMetadata(DateTime.Now));

        public DateTime ToDate
        {
            get { return (DateTime)GetValue(ToDateProperty); }
            set { SetValue(ToDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToDateProperty =
            DependencyProperty.Register("ToDate", typeof(DateTime), typeof(SemiTrimesterViewModel), new PropertyMetadata(DateTime.Now));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(SemiTrimesterViewModel), new PropertyMetadata(""));
    }
}
