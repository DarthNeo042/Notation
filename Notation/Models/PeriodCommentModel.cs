using Notation.Properties;
using Notation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class PeriodCommentModel
    {
        public static void Save(IEnumerable<PeriodCommentViewModel> periodComments, int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                foreach (IGrouping<Tuple<int, int>, PeriodCommentViewModel> periodCommentGroup in periodComments.GroupBy(c => new Tuple<int, int>(c.Student.Id, c.Period.Id)))
                {
                    using (SqlCommand command = new SqlCommand($"DELETE FROM PeriodComment WHERE [Year] = {year} AND IdStudent = {periodCommentGroup.Key.Item1} AND IdPeriod = {periodCommentGroup.Key.Item2}", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    foreach (PeriodCommentViewModel periodComment in periodCommentGroup)
                    {
                        using (SqlCommand command = new SqlCommand($"INSERT INTO PeriodComment([Year], StudiesReport, DisciplineReport, IdStudent, IdPeriod)"
                            + $" VALUES({year}, {(int)periodComment.StudiesReport}, {(int)periodComment.DisciplineReport}, {periodComment.Student.Id}, {periodComment.Period.Id})", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static PeriodCommentViewModel Read(PeriodViewModel period, StudentViewModel student)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [PeriodComment] WHERE [Year] = {period.Year} AND [IdPeriod] = {period.Id} AND [IdStudent] = {student.Id}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PeriodCommentViewModel()
                            {
                                Id = (int)reader["Id"],
                                DisciplineReport = (PeriodCommentViewModel.ReportEnum)(int)reader["DisciplineReport"],
                                StudiesReport = (PeriodCommentViewModel.ReportEnum)(int)reader["StudiesReport"],
                                Period = period,
                                Student = student,
                                Year = period.Year,
                            };
                        }
                    }
                }
            }

            return null;
        }

        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM PeriodComment WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
