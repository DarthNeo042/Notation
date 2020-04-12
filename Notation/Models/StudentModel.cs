using Notation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class StudentModel
    {
        public static IEnumerable<StudentViewModel> Read(int year, IEnumerable<ClassViewModel> Classes)
        {
            List<StudentViewModel> Students = new List<StudentViewModel>();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [Student] WHERE Year = {0} ORDER BY LastName, FirstName", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentViewModel student = new StudentViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                LastName = (string)reader["LastName"],
                                FirstName = (string)reader["FirstName"],
                                BirthDate = (DateTime)reader["BirthDate"],
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("IdClass")))
                            {
                                int idClass = (int)reader["IdClass"];
                                ClassViewModel _class = Classes.FirstOrDefault(c => c.Id == idClass);
                                if (_class != null)
                                {
                                    _class.Students.Add(student);
                                    student.Class = _class;
                                }
                            }
                            Students.Add(student);
                        }
                    }
                }
            }

            return Students;
        }

        public static void Save(StudentViewModel student)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (student.Id == 0)
                {
                    query = string.Format("INSERT INTO [Student]([Year], LastName, FirstName, BirthDate, IdClass) VALUES({0}, '{1}', '{2}', '{3}', {4})",
                        student.Year, student.LastName, student.FirstName, student.BirthDate.ToShortDateString(), student.Class != null ? student.Class.Id.ToString() : "NULL");
                }
                else
                {
                    query = string.Format("UPDATE [Student] SET LastName = '{0}', FirstName = '{1}', BirthDate = '{2}', IdClass = {3}"
                        + " WHERE [Student].Id = {4} AND [Student].[Year] = {5}",
                        student.LastName, student.FirstName, student.BirthDate.ToShortDateString(), student.Class != null ? student.Class.Id.ToString() : "NULL", student.Id, student.Year);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SaveClass(ClassViewModel _class)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                foreach (StudentViewModel student in _class.Students)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("UPDATE [Student] SET IdClass = {0} WHERE [Student].Id = {1} AND [Student].[Year] = {2}",
                        _class.Id, student.Id, student.Year), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void Delete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE [Student] WHERE Year = {0} AND Id = {1}", year, id), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
