using Notation.Settings;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class SemiTrimesterModel
    {
        public static IEnumerable<SemiTrimesterViewModel> Read(int year, IEnumerable<PeriodViewModel> periods)
        {
            List<SemiTrimesterViewModel> semiTrimesters = new List<SemiTrimesterViewModel>();

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [SemiTrimester] WHERE Year = {year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SemiTrimesterViewModel semiTrimester = new SemiTrimesterViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                            };

                            semiTrimester.Period1 = periods.FirstOrDefault(p => p.Id == (int)reader["idPeriod1"]);
                            if (!reader.IsDBNull(reader.GetOrdinal("IdPeriod2")))
                            {
                                semiTrimester.Period2 = periods.FirstOrDefault(p => p.Id == (int)reader["idPeriod2"]);
                            }
                            semiTrimester.Name = (string)reader["Name"];

                            semiTrimesters.Add(semiTrimester);
                        }
                    }
                }
            }

            return semiTrimesters.OrderBy(s => s.FromDate);
        }

        public static void Save(SemiTrimesterViewModel semiTrimester)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("UPDATE [SemiTrimester] SET Name = '{0}', IdPeriod1 = {1}, IdPeriod2 = {2} WHERE [SemiTrimester].Id = {3} AND [SemiTrimester].[Year] = {4}",
                        semiTrimester.Name, semiTrimester.Period1.Id, semiTrimester.Period2 != null ? semiTrimester.Period2.Id.ToString() : "NULL", semiTrimester.Id, semiTrimester.Year), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Save(IEnumerable<SemiTrimesterViewModel> semiTrimesters)
        {
            if (semiTrimesters.Any())
            {
                using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand($"DELETE FROM SemiTrimester WHERE [Year] = {semiTrimesters.First().Year}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    foreach (SemiTrimesterViewModel semiTrimester in semiTrimesters)
                    {
                        using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [SemiTrimester]([Year], Name, IdPeriod1, IdPeriod2) VALUES({0}, '{1}', {2}, {3})",
                            semiTrimester.Year, semiTrimester.Name, semiTrimester.Period1.Id, semiTrimester.Period2 != null ? semiTrimester.Period2.Id.ToString() : "NULL"), connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static bool CanDeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT COUNT(1) AS Count FROM SemiTrimesterComment WHERE Year = {year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (int)reader["Count"] == 0;
                        }
                    }
                }
            }
            return true;
        }

        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM SemiTrimester WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
