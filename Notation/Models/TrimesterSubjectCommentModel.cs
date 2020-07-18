using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class TrimesterSubjectCommentModel
    {
        public static TrimesterSubjectCommentViewModel Read(int trimester, StudentViewModel student, SubjectViewModel subject)
        {
            TrimesterSubjectCommentViewModel TrimesterSubjectComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT * FROM [TrimesterSubjectComment] WHERE IdStudent = '{0}' AND Trimester = {1} AND [Year] = {2} ORDER BY [Order]",
                        student.Id, trimester, student.Year);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TrimesterSubjectComment = new TrimesterSubjectCommentViewModel()
                            {
                                Id = (int)reader["Id"],
                                Comment = (string)reader["Comment"],
                                Order = (int)reader["Order"],
                                Student = student,
                                Subject = subject,
                                Trimester = trimester,
                                Year = student.Year,
                            };
                        }
                    }
                }
            }

            return TrimesterSubjectComment;
        }

        public static void Save(TrimesterSubjectCommentViewModel TrimesterSubjectComment)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (TrimesterSubjectComment.Id == 0)
                {
                    query = string.Format("INSERT INTO [TrimesterSubjectComment]([Year], Comment, Trimester, IdStudent, IdSubject) VALUES({0}, '{1}', {2}, {3}, {4})",
                        TrimesterSubjectComment.Year, TrimesterSubjectComment.Comment, TrimesterSubjectComment.Trimester, TrimesterSubjectComment.Student.Id, TrimesterSubjectComment.Subject.Id);
                }
                else
                {
                    query = string.Format("UPDATE [TrimesterSubjectComment] SET Comment = '{0}', Trimester = {1}, IdStudent = {2}, IdSubject = {3}"
                        + " WHERE Id = {4} AND [Year] = {5}", TrimesterSubjectComment.Comment, TrimesterSubjectComment.Trimester, TrimesterSubjectComment.Student.Id, TrimesterSubjectComment.Subject.Id);
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM TrimesterSubjectComment WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
