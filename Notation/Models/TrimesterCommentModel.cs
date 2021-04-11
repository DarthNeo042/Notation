using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public class TrimesterCommentModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string MainTeacherComment { get; set; }
        public string DivisionPrefectComment { get; set; }
        public int Trimester { get; set; }
        public int IdStudent { get; set; }

        public static TrimesterCommentModel Read(int trimester, StudentViewModel student)
        {
            TrimesterCommentModel TrimesterComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [TrimesterComment] WHERE IdStudent = '{student.Id}' AND Trimester = {trimester} AND [Year] = {student.Year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TrimesterComment = new TrimesterCommentModel()
                            {
                                Id = (int)reader["Id"],
                                DivisionPrefectComment = (string)reader["DivisionPrefectComment"],
                                MainTeacherComment = (string)reader["MainTeacherComment"],
                                IdStudent = student.Id,
                                Trimester = trimester,
                                Year = student.Year,
                            };
                        }
                    }
                }
            }

            return TrimesterComment;
        }

        public static void Save(TrimesterCommentModel trimesterComment)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (trimesterComment.Id == 0)
                {
                    query = "INSERT INTO [TrimesterComment]([Year], MainTeacherComment, DivisionPrefectComment, Trimester, IdStudent)"
                        + $" VALUES({trimesterComment.Year}, '{trimesterComment.MainTeacherComment.Replace("'", "''")}', '{trimesterComment.DivisionPrefectComment.Replace("'", "''")}',"
                        + $" {trimesterComment.Trimester}, {trimesterComment.IdStudent})";
                }
                else
                {
                    query = $"UPDATE [TrimesterComment] SET MainTeacherComment = '{trimesterComment.MainTeacherComment.Replace("'", "''")}', DivisionPrefectComment = '{trimesterComment.DivisionPrefectComment.Replace("'", "''")}',"
                        + $" Trimester = {trimesterComment.Trimester}, IdStudent = {trimesterComment.IdStudent} WHERE Id = {trimesterComment.Id} AND [Year] = {trimesterComment.Year}";
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
