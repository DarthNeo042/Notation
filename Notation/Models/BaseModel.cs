using System.Data.SqlClient;
using System.IO;

namespace Notation.Models
{
    public static class BaseModel
    {
        public static void Save(string filename)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"BACKUP DATABASE [NotationCollege] TO  DISK = N'{filename}' WITH NOFORMAT, NOINIT,"
                    + $" NAME = N'{Path.GetFileNameWithoutExtension(filename)}', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
