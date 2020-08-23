using Notation.Properties;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class TeacherModel
    {
        public static IEnumerable<TeacherViewModel> Read(int year)
        {
            List<TeacherViewModel> Teachers = new List<TeacherViewModel>();

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Teacher] WHERE Year = {year} ORDER BY [LastName], [FirstName]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Teachers.Add(new TeacherViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Login = (string)reader["Login"],
                                Password = (string)reader["Password"],
                                Title = (string)reader["Title"],
                            });
                        }
                    }
                }
            }

            return Teachers;
        }

        public static void Save(TeacherViewModel Teacher)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (Teacher.Id == 0)
                {
                    query = string.Format("INSERT INTO [Teacher]([Year], FirstName, LastName, [Login], [Password], [Title]) VALUES({0}, '{1}', '{2}', '{3}', '{4}', '{5}')",
                        Teacher.Year, Teacher.FirstName.Replace("'", "''"), Teacher.LastName.Replace("'", "''"), Teacher.Login, Teacher.Password, Teacher.Title);
                }
                else
                {
                    query = string.Format("UPDATE [Teacher] SET FirstName = '{0}', LastName = '{1}', [Login] = '{2}', [Password] = '{3}', [Title] = '{4}' "
                        + "WHERE [Teacher].Id = {5} AND [Teacher].[Year] = {6}",
                        Teacher.FirstName.Replace("'", "''"), Teacher.LastName.Replace("'", "''"), Teacher.Login, Teacher.Password, Teacher.Title, Teacher.Id, Teacher.Year);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool CanDelete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT COUNT(1) AS Count FROM Mark WHERE IdTeacher = {id} AND Year = {year}", connection))
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

        public static void Delete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE [Teacher] WHERE Year = {year} AND Id = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM Teacher WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static IEnumerable<int> Login(string login, string passord)
        {
            List<int> years = new List<int>();

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT [Year] FROM [Teacher] WHERE Login = '{login}' AND Password = '{passord}' ORDER BY [Year]", connection))
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

        public static TeacherViewModel Login(string login, string password, int year)
        {
            TeacherViewModel teacher = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT TOP 1 * FROM [Teacher] WHERE Login = '{login}' AND Password = '{password}' AND [Year] = {year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            teacher = new TeacherViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Login = (string)reader["Login"],
                                Password = (string)reader["Password"],
                                Title = (string)reader["Title"],
                            };
                        }
                    }
                }
            }

            return teacher;
        }
    }
}
