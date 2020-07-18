using Notation.Properties;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class ClassModel
    {
        public static IEnumerable<ClassViewModel> Read(int year, IEnumerable<TeacherViewModel> teachers, IEnumerable<LevelViewModel> levels)
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [Class] WHERE [Year] = {0} ORDER BY [Order]", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClassViewModel _class = new ClassViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                Order = (int)reader["Order"],
                                Name = (string)reader["Name"],
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("IdMainTeacher")))
                            {
                                _class.MainTeacher = teachers.FirstOrDefault(t => t.Id == (int)reader["IdMainTeacher"]);
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("IdLevel")))
                            {
                                _class.Level = levels.FirstOrDefault(l => l.Id == (int)reader["IdLevel"]);
                            }
                            classes.Add(_class);
                        }
                    }
                }
            }

            return classes;
        }

        public static void Save(ClassViewModel _class)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (_class.Id == 0)
                {
                    query = string.Format("INSERT INTO [Class]([Year], [Order], [Name], [IdLevel], [IdMainTeacher]) VALUES({0}, {1}, '{2}', {3}, {4})",
                        _class.Year, _class.Order, _class.Name, _class.Level == null ? "NULL": _class.Level.Id.ToString(), _class.MainTeacher == null ? "NULL" : _class.MainTeacher.Id.ToString());
                }
                else
                {
                    query = string.Format("UPDATE [Class] SET Name = '{0}', [Order] = {1}, [IdLevel] = {2}, [IdMainTeacher] = {3} "
                        + "WHERE [Class].Id = {4} AND [Class].[Year] = {5}",
                        _class.Name, _class.Order, _class.Level == null ? "NULL" : _class.Level.Id.ToString(), _class.MainTeacher == null ? "NULL" : _class.MainTeacher.Id.ToString(), _class.Id, _class.Year);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE [Class] WHERE Year = {0} AND Id = {1}", year, id), connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM Class WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
