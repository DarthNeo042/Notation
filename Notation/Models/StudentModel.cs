using Notation.Settings;
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

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Student] WHERE Year = {year} ORDER BY LastName, FirstName", connection))
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
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (student.Id == 0)
                {
                    query = string.Format("INSERT INTO [Student]([Year], LastName, FirstName, BirthDate, IdClass) VALUES({0}, '{1}', '{2}', '{3}', {4})",
                        student.Year, student.LastName.Replace("'", "''"), student.FirstName.Replace("'", "''"), student.BirthDate.ToShortDateString(), student.Class != null ? student.Class.Id.ToString() : "NULL");
                }
                else
                {
                    query = string.Format("UPDATE [Student] SET LastName = '{0}', FirstName = '{1}', BirthDate = '{2}', IdClass = {3}"
                        + " WHERE [Student].Id = {4} AND [Student].[Year] = {5}",
                        student.LastName.Replace("'", "''"), student.FirstName.Replace("'", "''"), student.BirthDate.ToShortDateString(), student.Class != null ? student.Class.Id.ToString() : "NULL", student.Id, student.Year);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SaveClass(ClassViewModel _class)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                foreach (StudentViewModel student in _class.Students)
                {
                    using (SqlCommand command = new SqlCommand($"UPDATE [Student] SET IdClass = {_class.Id} WHERE [Student].Id = {student.Id} AND [Student].[Year] = {student.Year}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                IEnumerable<StudentViewModel> unassignedStudents = MainViewModel.Instance.Parameters.Students.Where(s => !_class.Students.Any(s2 => s2.Id == s.Id));
                foreach (ClassViewModel _class2 in MainViewModel.Instance.Parameters.Classes.Where(c => c.Id != _class.Id))
                {
                    unassignedStudents = unassignedStudents.Where(s => !_class2.Students.Any(s2 => s2.Id == s.Id));
                }
                foreach (StudentViewModel student in unassignedStudents)
                {
                    using (SqlCommand command = new SqlCommand($"UPDATE [Student] SET IdClass = NULL WHERE [Student].Id = {student.Id} AND [Student].[Year] = {student.Year}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static bool CanDelete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT(SELECT COUNT(1) FROM Mark WHERE IdStudent = {id} AND Year = {year})"
                    + $" + (SELECT COUNT(1) FROM PeriodComment WHERE IdStudent = {id} AND Year = {year})"
                    + $" + (SELECT COUNT(1) FROM SemiTrimesterComment WHERE IdStudent = {id} AND Year = {year})"
                    + $" + (SELECT COUNT(1) FROM TrimesterSubjectComment WHERE IdStudent = {id} AND Year = {year})"
                    + $" + (SELECT COUNT(1) FROM TrimesterComment WHERE IdStudent = {id} AND Year = {year}) AS Count", connection))
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
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE [Student] WHERE Year = {year} AND Id = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteMarksAndComments(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE [Mark] WHERE Year = {year} AND IdStudent = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand($"DELETE [PeriodComment] WHERE Year = {year} AND IdStudent = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand($"DELETE [SemiTrimesterComment] WHERE Year = {year} AND IdStudent = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand($"DELETE [TrimesterSubjectComment] WHERE Year = {year} AND IdStudent = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand($"DELETE [TrimesterComment] WHERE Year = {year} AND IdStudent = {id}", connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM Student WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
