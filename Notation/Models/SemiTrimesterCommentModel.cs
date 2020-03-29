using Notation.Properties;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class SemiTrimesterCommentModel
    {
        public static SemiTrimesterCommentViewModel Read(StudentViewModel student, SemiTrimesterViewModel semiTrimester, int year)
        {
            SemiTrimesterCommentViewModel semiTrimesterComment = null;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT * FROM [SemiTrimesterComment] WHERE IdStudent = '{0}' AND IdSemiTrimester = '{1}' AND [Year] = {2} ORDER BY [Order]",
                        student.Id, semiTrimester.IdPeriod, year);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semiTrimesterComment = new SemiTrimesterCommentViewModel()
                            {
                                Id = (int)reader["Id"],
                                Order = (int)reader["Order"],
                                IdStudent = student.Id,
                                IdSemiTrimester = semiTrimester.IdPeriod,
                                Year = year,
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

                string update = "";
                if (semiTrimesterComment.Id == 0)
                {
                    update = string.Format("INSERT INTO [SemiTrimesterComment]([Year], [Order], MainTeacherComment, DivisionPrefectComment, IdSemiTrimester, IdStudent)"
                        + " VALUES({0}, {1}, '{2}', '{3}', {4}, {5})", semiTrimesterComment.Year, semiTrimesterComment.Order,
                        semiTrimesterComment.MainTeacherComment, semiTrimesterComment.DivisionPrefectComment, semiTrimesterComment.IdSemiTrimester, semiTrimesterComment.IdStudent);
                }
                else
                {
                    update = string.Format("UPDATE [SemiTrimesterComment] SET [Order] = {0}, MainTeacherComment = '{1}', DivisionPrefectComment = '{2}', IdSemiTrimester = {3}, IdStudent = {4}"
                        + " WHERE Id = {5} AND [Year] = {6}", semiTrimesterComment.Order, semiTrimesterComment.MainTeacherComment,
                        semiTrimesterComment.DivisionPrefectComment, semiTrimesterComment.IdSemiTrimester, semiTrimesterComment.IdStudent);
                }
                using (SqlCommand command = new SqlCommand(update, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
