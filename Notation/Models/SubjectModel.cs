﻿using Notation.Settings;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class SubjectModel
    {
        public static IEnumerable<SubjectViewModel> ReadParents(int year)
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>();

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Subject] WHERE Year = {year} AND ParentSubjectId IS NULL ORDER BY [Order]", connection))
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

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Subject] WHERE Year = {year} AND ParentSubjectId IS NOT NULL ORDER BY [Order] DESC", connection))
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
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (subject.Id == 0)
                {
                    query = "INSERT INTO [Subject]([Year], [Order], Name, Coefficient, [Option], ParentSubjectId)"
                        + $" VALUES({subject.Year}, {subject.Order}, '{subject.Name}', {subject.Coefficient.ToString().Replace(',', '.')},"
                        + $" {(subject.Option ? 1 : 0)}, {(subject.ParentSubject != null ? subject.ParentSubject.Id.ToString() : "NULL")})";
                }
                else
                {
                    query = $"UPDATE [Subject] SET Name = '{subject.Name}', [Order] = {subject.Order}, Coefficient = {subject.Coefficient.ToString().Replace(',', '.')},"
                        + $" [Option] = {(subject.Option ? 1 : 0)}, ParentSubjectId = {(subject.ParentSubject != null ? subject.ParentSubject.Id.ToString() : "NULL")}"
                        + $" WHERE [Subject].Id = {subject.Id} AND [Subject].[Year] = {subject.Year}";
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool CanDelete(int year, int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT(SELECT COUNT(1) FROM Mark WHERE IdSubject = {id} AND Year = {year})"
                    + $" + (SELECT COUNT(1) FROM TrimesterSubjectComment WHERE IdSubject = {id} AND Year = {year}) AS Count", connection))
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

                using (SqlCommand command = new SqlCommand($"DELETE [Subject] WHERE Year = {year} AND Id = {id}", connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM Subject WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
