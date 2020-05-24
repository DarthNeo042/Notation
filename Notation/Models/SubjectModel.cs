using Notation.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class SubjectModel
    {
        public static IEnumerable<SubjectViewModel> ReadParents(int year)
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>();

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [Subject] WHERE Year = {0} AND ParentSubjectId IS NULL ORDER BY [Order]", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new SubjectViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                Order = (int)reader["Order"],
                                Name = (string)reader["Name"],
                                Coefficient = (double)(decimal)reader["Coefficient"],
                                Option = (bool)reader["Option"],
                            });
                        }
                    }
                }
            }

            return subjects;
        }

        public static IEnumerable<SubjectViewModel> ReadChildren(int year, IEnumerable<SubjectViewModel> parentsSubjects)
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>(parentsSubjects);

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [Subject] WHERE Year = {0} AND ParentSubjectId IS NOT NULL ORDER BY [Order] DESC", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubjectViewModel parentSubject = parentsSubjects.FirstOrDefault(s => s.Id == (int)reader["ParentSubjectId"]);
                            SubjectViewModel subject = new SubjectViewModel()
                            {
                                Year = (int)reader["Year"],
                                Id = (int)reader["Id"],
                                Order = (int)reader["Order"],
                                Name = (string)reader["Name"],
                                Coefficient = (double)(decimal)reader["Coefficient"],
                                Option = (bool)reader["Option"],
                                ParentSubject = parentSubject,
                            };
                            subjects.Insert(subjects.IndexOf(parentSubject) + 1, subject);
                            parentSubject.ChildrenSubjects.Add(subject);
                        }
                    }
                }
            }

            return subjects;
        }

        public static void Save(SubjectViewModel subject)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (subject.Id == 0)
                {
                    query = string.Format("INSERT INTO [Subject]([Year], [Order], Name, Coefficient, [Option], ParentSubjectId) VALUES({0}, {1}, '{2}', {3}, {4}, {5})",
                        subject.Year, subject.Order, subject.Name, subject.Coefficient.ToString().Replace(',', '.'), subject.Option ? 1 : 0,
                        subject.ParentSubject != null ? subject.ParentSubject.Id.ToString() : "NULL");
                }
                else
                {
                    query = string.Format("UPDATE [Subject] SET Name = '{0}', [Order] = {1}, Coefficient = {2}, [Option] = {3}, ParentSubjectId = {4}"
                        + " WHERE [Subject].Id = {5} AND [Subject].[Year] = {6}",
                        subject.Name, subject.Order, subject.Coefficient.ToString().Replace(',', '.'), subject.Option ? 1 : 0,
                        subject.ParentSubject != null ? subject.ParentSubject.Id.ToString() : "NULL", subject.Id, subject.Year);
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

                using (SqlCommand command = new SqlCommand(string.Format("DELETE [Subject] WHERE Year = {0} AND Id = {1}", year, id), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
