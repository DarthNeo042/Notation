using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class LevelModel
    {
        public static IEnumerable<LevelViewModel> Read(int year)
        {
            List<LevelViewModel> Levels = new List<LevelViewModel>();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [Level] WHERE Year = {0} ORDER BY [Order]", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Levels.Add(new LevelViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                Order = (int)reader["Order"],
                                Name = (string)reader["Name"],
                            });
                        }
                    }
                }
            }

            return Levels;
        }

        public static void Save(LevelViewModel Level)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (Level.Id == 0)
                {
                    query = string.Format("INSERT INTO [Level]([Year], [Order], Name) VALUES({0}, {1}, '{2}')", Level.Year, Level.Order, Level.Name);
                }
                else
                {
                    query = string.Format("UPDATE [Level] SET Name = '{0}', [Order] = {1} WHERE [Level].Id = {2} AND [Level].[Year] = {3}", Level.Name, Level.Order, Level.Id, Level.Year);
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

                using (SqlCommand command = new SqlCommand(string.Format("DELETE [Level] WHERE Year = {0} AND Id = {1}", year, id), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
