using Notation.Settings;
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

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Level] WHERE Year = {year} ORDER BY [Order]", connection))
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
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (Level.Id == 0)
                {
                    query = $"INSERT INTO [Level]([Year], [Order], Name) VALUES({Level.Year}, {Level.Order}, '{Level.Name}')";
                }
                else
                {
                    query = $"UPDATE [Level] SET Name = '{Level.Name}', [Order] = {Level.Order} WHERE [Level].Id = {Level.Id} AND [Level].[Year] = {Level.Year}";
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE [Level] WHERE Year = {year} AND Id = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM Level WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
