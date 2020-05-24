using System;
using System.Linq;
using System.Windows;

namespace Notation.ViewModels
{
    public class PeriodViewModel : BaseViewModel
    {
        public int Trimester
        {
            get { return (int)GetValue(TrimesterProperty); }
            set { SetValue(TrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimesterProperty =
            DependencyProperty.Register("Trimester", typeof(int), typeof(PeriodViewModel), new PropertyMetadata(1));

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(PeriodViewModel), new PropertyMetadata(1));

        public DateTime FromDate
        {
            get { return (DateTime)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime), typeof(PeriodViewModel), new PropertyMetadata(DateTime.Now));

        public DateTime ToDate
        {
            get { return (DateTime)GetValue(ToDateProperty); }
            set { SetValue(ToDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToDateProperty =
            DependencyProperty.Register("ToDate", typeof(DateTime), typeof(PeriodViewModel), new PropertyMetadata(DateTime.Now));

        public void InitNumber()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (!MainViewModel.Instance.Parameters.Periods.Any(p => p.Number == i))
                {
                    Number = i;
                    break;
                }
            }
            if (MainViewModel.Instance.Parameters.Periods.Any())
            {
                Trimester = MainViewModel.Instance.Parameters.Periods.Max(p => p.Trimester);
            }
        }
    }
}
