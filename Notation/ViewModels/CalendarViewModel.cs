using Notation.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Notation.ViewModels
{
    public class CalendarViewModel : DependencyObject
    {
        public string PeriodsSummary
        {
            get { return (string)GetValue(PeriodsSummaryProperty); }
            set { SetValue(PeriodsSummaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PeriodsSummary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeriodsSummaryProperty =
            DependencyProperty.Register("PeriodsSummary", typeof(string), typeof(CalendarViewModel), new PropertyMetadata(""));

        public string SemiTrimestersSummary
        {
            get { return (string)GetValue(SemiTrimestersSummaryProperty); }
            set { SetValue(SemiTrimestersSummaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SemiTrimestersSummary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SemiTrimestersSummaryProperty =
            DependencyProperty.Register("SemiTrimestersSummary", typeof(string), typeof(CalendarViewModel), new PropertyMetadata(""));

        public string TrimestersSummary
        {
            get { return (string)GetValue(TrimestersSummaryProperty); }
            set { SetValue(TrimestersSummaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrimestersSummary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimestersSummaryProperty =
            DependencyProperty.Register("TrimestersSummary", typeof(string), typeof(CalendarViewModel), new PropertyMetadata(""));

        public string DatesSummary
        {
            get { return (string)GetValue(DatesSummaryProperty); }
            set { SetValue(DatesSummaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DatesSummary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatesSummaryProperty =
            DependencyProperty.Register("DatesSummary", typeof(string), typeof(CalendarViewModel), new PropertyMetadata(""));

        public void LoadCalendarSummaries(IEnumerable<PeriodViewModel> periods)
        {
            DatesSummary = "";
            PeriodsSummary = "";
            SemiTrimestersSummary = "";
            TrimestersSummary = "";

            int trimester = 0;
            PeriodViewModel lastPeriod = null;
            foreach (PeriodViewModel period in periods.OrderBy(p => p.Trimester).ThenBy(p => p.Number))
            {
                DatesSummary += string.Format("Du {0} au {1}\r\n", period.FromDate.ToShortDateString(), period.ToDate.ToShortDateString());
                PeriodsSummary += string.Format("Période {0}\r\n", period.Number);
                if (lastPeriod == null)
                {
                    lastPeriod = period;
                }
                else
                {
                    if (lastPeriod.Trimester == period.Trimester)
                    {
                        SemiTrimestersSummary += string.Format("Demi-trimestre de {0}\r\n   |\r\n", MonthUtils.Name(period.ToDate.Month));
                        lastPeriod = null;
                    }
                    else
                    {
                        SemiTrimestersSummary += string.Format("Demi-trimestre de {0}\r\n", MonthUtils.Name(lastPeriod.ToDate.Month));
                        lastPeriod = period;
                    }
                }
                if (trimester != period.Trimester)
                {
                    TrimestersSummary += string.Format("Trimestre {0}\r\n", period.Trimester);
                    trimester = period.Trimester;
                }
                else
                {
                    TrimestersSummary += "   |\r\n";
                }
            }
            if (lastPeriod != null)
            {
                SemiTrimestersSummary += string.Format("Demi-trimestre de {0}\r\n", MonthUtils.Name(lastPeriod.ToDate.Month));
            }
        }
    }
}
