using Notation.Properties;
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

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [SemiTrimesterComment] WHERE IdStudent = {student.Id} AND IdSemiTrimester = {semiTrimester.Id}"
                    + $" AND [Year] = {semiTrimester.Year}", connection))
                {
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
                    query = "INSERT INTO [SemiTrimesterComment]([Year], MainTeacherComment, DivisionPrefectComment, IdSemiTrimester, IdStudent)"
                        + $" VALUES({semiTrimesterComment.Year}, '{semiTrimesterComment.MainTeacherComment}', '{semiTrimesterComment.DivisionPrefectComment}',"
                        + $" {semiTrimesterComment.SemiTrimester.Id}, {semiTrimesterComment.Student.Id})";
                }
                else
                {
                    query = $"UPDATE [SemiTrimesterComment] SET MainTeacherComment = '{semiTrimesterComment.MainTeacherComment}', DivisionPrefectComment = '{semiTrimesterComment.DivisionPrefectComment}',"
                        + $" IdSemiTrimester = {semiTrimesterComment.SemiTrimester.Id}, IdStudent = {semiTrimesterComment.Student.Id} WHERE Id = {semiTrimesterComment.Id} AND [Year] = {semiTrimesterComment.Year}";
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
