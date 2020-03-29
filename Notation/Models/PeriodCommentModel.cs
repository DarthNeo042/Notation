using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class PeriodCommentModel
    {
        private struct Group
        {
            public int IdStudent;
            public int IdPeriod;
        }

        public static void Save(IEnumerable<PeriodCommentViewModel> periodComments, int year)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                foreach (IGrouping<Group, PeriodCommentViewModel> periodCommentGroup in periodComments.GroupBy(c => new Group() { IdStudent = c.IdStudent, IdPeriod = c.IdPeriod }))
                {
                    using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM PeriodComment WHERE [Year] = {0} AND IdStudent = {1} AND IdPeriod = {2}",
                        year, periodCommentGroup.Key.IdStudent, periodCommentGroup.Key.IdPeriod), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    foreach (PeriodCommentViewModel periodComment in periodCommentGroup)
                    {
                        using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO PeriodComment([Year], StudiesReport, DisciplineReport, IdStudent, IdPeriod) VALUES({0}, {1}, {2}, {3}, {4})",
                            year, periodComment.StudiesReport, periodComment.DisciplineReport, periodComment.IdStudent, periodComment.IdPeriod), connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
