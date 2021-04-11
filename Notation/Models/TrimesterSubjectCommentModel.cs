using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public class TrimesterSubjectCommentModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Comment { get; set; }
        public int Trimester { get; set; }
        public int IdSubject { get; set; }
        public int IdStudent { get; set; }

        public static TrimesterSubjectCommentModel Read(int trimester, StudentViewModel student, SubjectViewModel subject)
        {
            TrimesterSubjectCommentModel TrimesterSubjectComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [TrimesterSubjectComment] WHERE IdStudent = {student.Id} AND IdSubject = {subject.Id}"
                    + $" AND Trimester = {trimester} AND [Year] = {student.Year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TrimesterSubjectComment = new TrimesterSubjectCommentModel()
                            {
                                Id = (int)reader["Id"],
                                Comment = (string)reader["Comment"],
                                IdStudent = student.Id,
                                IdSubject = subject.Id,
                                Trimester = trimester,
                                Year = student.Year,
                            };
                        }
                    }
                }
            }

            return TrimesterSubjectComment;
        }

        public static void Save(TrimesterSubjectCommentModel TrimesterSubjectComment)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (TrimesterSubjectComment.Id == 0)
                {
                    query = "INSERT INTO [TrimesterSubjectComment]([Year], Comment, Trimester, IdStudent, IdSubject)"
                        + $" VALUES({TrimesterSubjectComment.Year}, '{TrimesterSubjectComment.Comment.Replace("'", "''")}', {TrimesterSubjectComment.Trimester},"
                        + $" {TrimesterSubjectComment.IdStudent}, {TrimesterSubjectComment.IdSubject})";
                }
                else
                {
                    query = $"UPDATE [TrimesterSubjectComment] SET Comment = '{TrimesterSubjectComment.Comment.Replace("'", "''")}', Trimester = {TrimesterSubjectComment.Trimester},"
                        + $" IdStudent = {TrimesterSubjectComment.IdStudent}, IdSubject = {TrimesterSubjectComment.IdSubject} WHERE Id = {TrimesterSubjectComment.Id} AND [Year] = {TrimesterSubjectComment.Year}";
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
