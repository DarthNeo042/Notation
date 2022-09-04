using Notation.Settings;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public class SemiTrimesterCommentModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string MainTeacherComment { get; set; }
        public string DivisionPrefectComment { get; set; }
        public int IdSemiTrimester { get; set; }
        public int IdStudent { get; set; }

        public static SemiTrimesterCommentModel Read(SemiTrimesterViewModel semiTrimester, StudentViewModel student)
        {
            SemiTrimesterCommentModel semiTrimesterComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [SemiTrimesterComment] WHERE IdStudent = {student.Id} AND IdSemiTrimester = {semiTrimester.Id}"
                    + $" AND [Year] = {semiTrimester.Year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semiTrimesterComment = new SemiTrimesterCommentModel()
                            {
                                Id = (int)reader["Id"],
                                DivisionPrefectComment = (string)reader["DivisionPrefectComment"],
                                MainTeacherComment = (string)reader["MainTeacherComment"],
                                IdStudent = student.Id,
                                IdSemiTrimester = semiTrimester.Id,
                                Year = semiTrimester.Year,
                            };
                        }
                    }
                }
            }

            return semiTrimesterComment;
        }

        public static void Save(SemiTrimesterCommentModel semiTrimesterComment)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                string query = "";
                if (semiTrimesterComment.Id == 0)
                {
                    query = "INSERT INTO [SemiTrimesterComment]([Year], MainTeacherComment, DivisionPrefectComment, IdSemiTrimester, IdStudent)"
                        + $" VALUES({semiTrimesterComment.Year}, '{semiTrimesterComment.MainTeacherComment.Replace("'", "''")}', '{semiTrimesterComment.DivisionPrefectComment.Replace("'", "''")}',"
                        + $" {semiTrimesterComment.IdSemiTrimester}, {semiTrimesterComment.IdStudent})";
                }
                else
                {
                    query = $"UPDATE [SemiTrimesterComment] SET MainTeacherComment = '{semiTrimesterComment.MainTeacherComment.Replace("'", "''")}', DivisionPrefectComment = '{semiTrimesterComment.DivisionPrefectComment.Replace("'", "''")}',"
                        + $" IdSemiTrimester = {semiTrimesterComment.IdSemiTrimester}, IdStudent = {semiTrimesterComment.IdStudent} WHERE Id = {semiTrimesterComment.Id} AND [Year] = {semiTrimesterComment.Year}";
                }
                using (SqlCommand command = new SqlCommand(query, connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM SemiTrimesterComment WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
