using Notation.Settings;
using Notation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public class PeriodCommentModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int StudiesReport { get; set; }
        public int DisciplineReport { get; set; }
        public int IdPeriod { get; set; }
        public int IdStudent { get; set; }

        public static void Save(IEnumerable<PeriodCommentModel> periodComments, int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                foreach (IGrouping<Tuple<int, int>, PeriodCommentModel> periodCommentGroup in periodComments.GroupBy(c => new Tuple<int, int>(c.IdStudent, c.IdPeriod)))
                {
                    using (SqlCommand command = new SqlCommand($"DELETE FROM PeriodComment WHERE [Year] = {year} AND IdStudent = {periodCommentGroup.Key.Item1} AND IdPeriod = {periodCommentGroup.Key.Item2}", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    foreach (PeriodCommentModel periodComment in periodCommentGroup)
                    {
                        using (SqlCommand command = new SqlCommand($"INSERT INTO PeriodComment([Year], StudiesReport, DisciplineReport, IdStudent, IdPeriod)"
                            + $" VALUES({year}, {(int)periodComment.StudiesReport}, {(int)periodComment.DisciplineReport}, {periodComment.IdStudent}, {periodComment.IdPeriod})", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static PeriodCommentModel Read(PeriodViewModel period, StudentViewModel student)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [PeriodComment] WHERE [Year] = {period.Year} AND [IdPeriod] = {period.Id} AND [IdStudent] = {student.Id}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PeriodCommentModel()
                            {
                                Id = (int)reader["Id"],
                                DisciplineReport = (int)reader["DisciplineReport"],
                                StudiesReport = (int)reader["StudiesReport"],
                                IdPeriod = period.Id,
                                IdStudent = student.Id,
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
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
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
