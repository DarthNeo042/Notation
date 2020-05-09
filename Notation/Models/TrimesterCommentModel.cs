using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class TrimesterCommentModel
    {
        public static TrimesterCommentViewModel Read(StudentViewModel student, int trimester)
        {
            TrimesterCommentViewModel TrimesterComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT * FROM [TrimesterComment] WHERE IdStudent = '{0}' AND Trimester = {1} AND [Year] = {2} ORDER BY [Order]",
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
                                Order = (int)reader["Order"],
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
                    query = string.Format("INSERT INTO [TrimesterComment]([Year], [Order], MainTeacherComment, DivisionPrefectComment, Trimester, IdStudent)"
                        + " VALUES({0}, {1}, '{2}', '{3}', {4}, {5})", trimesterComment.Year, trimesterComment.Order,
                        trimesterComment.MainTeacherComment, trimesterComment.DivisionPrefectComment, trimesterComment.Trimester, trimesterComment.Student.Id);
                }
                else
                {
                    query = string.Format("UPDATE [TrimesterComment] SET [Order] = {0}, MainTeacherComment = '{1}', DivisionPrefectComment = '{2}', Trimester = {3}, IdStudent = {4}"
                        + " WHERE Id = {5} AND [Year] = {6}", trimesterComment.Order, trimesterComment.MainTeacherComment,
                        trimesterComment.DivisionPrefectComment, trimesterComment.Trimester, trimesterComment.Student.Id);
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
