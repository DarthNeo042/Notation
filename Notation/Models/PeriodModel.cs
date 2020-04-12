using Notation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class PeriodModel
    {
        public static IEnumerable<PeriodViewModel> Read(int year)
        {
            List<PeriodViewModel> periods = new List<PeriodViewModel>();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [Period] WHERE Year = {0} ORDER BY [Trimester], [Number]", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            periods.Add(new PeriodViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                FromDate = (DateTime)reader["FromDate"],
                                Number = (int)reader["Number"],
                                ToDate = (DateTime)reader["ToDate"],
                                Trimester = (int)reader["Trimester"],
                            });
                        }
                    }
                }
            }

            return periods;
        }

        public static void Save(PeriodViewModel period)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (period.Id == 0)
                {
                    query = string.Format("INSERT INTO [Period]([Year], Trimester, Number, FromDate, ToDate) VALUES({0}, {1}, {2}, '{3}', '{4}')",
                        period.Year, period.Trimester, period.Number, period.FromDate.ToShortDateString(), period.ToDate.ToShortDateString());
                }
                else
                {
                    query = string.Format("UPDATE [Period] SET Trimester = {0}, Number = {1}, FromDate = '{2}', ToDate = '{3}' WHERE [Period].Id = {4} AND [Period].[Year] = {5}",
                        period.Trimester, period.Number, period.FromDate.ToShortDateString(), period.ToDate.ToShortDateString(), period.Id, period.Year);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE [Period] WHERE Year = {0} AND Id = {1}", year, id) , connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
