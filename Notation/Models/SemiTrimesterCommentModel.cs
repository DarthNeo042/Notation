﻿using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class SemiTrimesterCommentModel
    {
        public static SemiTrimesterCommentViewModel Read(SemiTrimesterViewModel semiTrimester, StudentViewModel student)
        {
            SemiTrimesterCommentViewModel semiTrimesterComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT * FROM [SemiTrimesterComment] WHERE IdStudent = {0} AND IdSemiTrimester = {1} AND [Year] = {2}",
                        student.Id, semiTrimester.Id, semiTrimester.Year);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semiTrimesterComment = new SemiTrimesterCommentViewModel()
                            {
                                Id = (int)reader["Id"],
                                DivisionPrefectComment = (string)reader["DivisionPrefectComment"],
                                MainTeacherComment = (string)reader["MainTeacherComment"],
                                Student = student,
                                SemiTrimester = semiTrimester,
                                Year = semiTrimester.Year,
                            };
                        }
                    }
                }
            }

            return semiTrimesterComment;
        }

        public static void Save(SemiTrimesterCommentViewModel semiTrimesterComment)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (semiTrimesterComment.Id == 0)
                {
                    query = string.Format("INSERT INTO [SemiTrimesterComment]([Year], MainTeacherComment, DivisionPrefectComment, IdSemiTrimester, IdStudent) VALUES({0}, '{1}', '{2}', {3}, {4})",
                        semiTrimesterComment.Year, semiTrimesterComment.MainTeacherComment, semiTrimesterComment.DivisionPrefectComment, semiTrimesterComment.SemiTrimester.Id, semiTrimesterComment.Student.Id);
                }
                else
                {
                    query = string.Format("UPDATE [SemiTrimesterComment] SET MainTeacherComment = '{0}', DivisionPrefectComment = '{1}', IdSemiTrimester = {2}, IdStudent = {3} WHERE Id = {4} AND [Year] = {5}",
                        semiTrimesterComment.MainTeacherComment, semiTrimesterComment.DivisionPrefectComment, semiTrimesterComment.SemiTrimester.Id, semiTrimesterComment.Student.Id, semiTrimesterComment.Id, semiTrimesterComment.Year);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM SemiTrimesterComment WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
