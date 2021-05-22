using Notation.Properties;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class YearModel
    {
        public static IEnumerable<int> List()
        {
            List<int> years = new List<int>();

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Year", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            years.Add((int)reader["Year"]);
                        }
                    }
                }
            }

            return years;
        }

        public static void Create(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"INSERT INTO Year(Year) VALUES ({year})", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetCurrentYear()
        {
            int year = 0;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT CASE WHEN (SELECT COUNT(1) FROM [Period] WHERE FromDate <= GETDATE() AND ToDate >= GETDATE()) <> 0"
                    + " THEN (SELECT TOP 1 [Year] FROM [Period] WHERE FromDate <= GETDATE() AND ToDate >= GETDATE()) ELSE (SELECT MAX(Year) FROM Year) END AS [Year]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            year = reader.IsDBNull(reader.GetOrdinal("Year")) ? 0 : (int)reader["Year"];
                        }
                    }
                }
            }

            return year;
        }

        public static void Delete(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM Year WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
