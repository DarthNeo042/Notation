using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class TrimesterCommentModel
    {
        public static TrimesterCommentViewModel Read(int trimester, StudentViewModel student)
        {
            TrimesterCommentViewModel TrimesterComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [TrimesterComment] WHERE IdStudent = '{student.Id}' AND Trimester = {trimester} AND [Year] = {student.Year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TrimesterComment = new TrimesterCommentViewModel()
                            {
                                Id = (int)reader["Id"],
                                DivisionPrefectComment = (string)reader["DivisionPrefectComment"],
                                MainTeacherComment = (string)reader["MainTeacherComment"],
                                Student = student,
                                Trimester = trimester,
                                Year = student.Year,
                            };
                        }
                    }
                }
            }

            return TrimesterComment;
        }

        public static void Save(TrimesterCommentViewModel trimesterComment)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (trimesterComment.Id == 0)
                {
                    query = "INSERT INTO [TrimesterComment]([Year], MainTeacherComment, DivisionPrefectComment, Trimester, IdStudent)"
                        + $" VALUES({trimesterComment.Year}, '{trimesterComment.MainTeacherComment}', '{trimesterComment.DivisionPrefectComment}', {trimesterComment.Trimester}, {trimesterComment.Student.Id})";
                }
                else
                {
                    query = $"UPDATE [TrimesterComment] SET MainTeacherComment = '{trimesterComment.MainTeacherComment}', DivisionPrefectComment = '{trimesterComment.DivisionPrefectComment}',"
                        + $" Trimester = {trimesterComment.Trimester}, IdStudent = {trimesterComment.Student.Id} WHERE Id = {trimesterComment.Id} AND [Year] = {trimesterComment.Year}";
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM TrimesterComment WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
