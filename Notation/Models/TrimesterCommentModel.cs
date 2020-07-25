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

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT * FROM [TrimesterComment] WHERE IdStudent = '{0}' AND Trimester = {1} AND [Year] = {2}",
                        student.Id, trimester, student.Year);

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
                    query = string.Format("INSERT INTO [TrimesterComment]([Year], MainTeacherComment, DivisionPrefectComment, Trimester, IdStudent) VALUES({0}, '{1}', '{2}', {3}, {4})",
                        trimesterComment.Year, trimesterComment.MainTeacherComment, trimesterComment.DivisionPrefectComment, trimesterComment.Trimester, trimesterComment.Student.Id);
                }
                else
                {
                    query = string.Format("UPDATE [TrimesterComment] SET MainTeacherComment = '{0}', DivisionPrefectComment = '{1}', Trimester = {2}, IdStudent = {3} WHERE Id = {4} AND [Year] = {5}",
                        trimesterComment.MainTeacherComment, trimesterComment.DivisionPrefectComment, trimesterComment.Trimester, trimesterComment.Student.Id, trimesterComment.Id, trimesterComment.Year);
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
