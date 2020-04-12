using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class YearModel
    {
        public static IEnumerable<int> Read()
        {
            List<int> years = new List<int>();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
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
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
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

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT CASE WHEN (SELECT COUNT(1) FROM [Period] WHERE FromDate <= GETDATE() AND ToDate >= GETDATE()) <> 0"
                    + " THEN (SELECT TOP 1 [Year] FROM [Period] WHERE FromDate <= GETDATE() AND ToDate >= GETDATE()) ELSE YEAR(GETDATE()) END AS [Year]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            year = (int)reader["Year"];
                        }
                    }
                }
            }

            return year;
        }
    }
}
