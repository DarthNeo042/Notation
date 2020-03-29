using Notation.Utils;
using System;
using System.Windows;

namespace Notation.ViewModels
{
    public class SemiTrimesterViewModel : DependencyObject
    {
        public int IdPeriod
        {
            get { return (int)GetValue(IdPeriodProperty); }
            set { SetValue(IdPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdPeriodProperty =
            DependencyProperty.Register("IdPeriod", typeof(int), typeof(SemiTrimesterViewModel), new PropertyMetadata(0));

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

        public string Month
        {
            get
            {
                return MonthUtils.Name(ToDate.Month);
            }
        }
    }
}
