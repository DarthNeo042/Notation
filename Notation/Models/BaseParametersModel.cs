using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class BaseParametersModel
    {
        public static BaseParametersViewModel Read()
        {
            BaseParametersViewModel baseParameters = null;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM [BaseParameters]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            baseParameters = new BaseParametersViewModel()
                            {
                                ColorR = (byte)(int)reader["ColorR"],
                                ColorG = (byte)(int)reader["ColorG"],
                                ColorB = (byte)(int)reader["ColorB"],
                                AdminLogin = (string)reader["AdminLogin"],
                                AdminPassword = (string)reader["AdminPassword"],
                                Name = (string)reader["Name"],
                            };
                        }
                    }
                }
            }

            return baseParameters;
        }

        public static void Create(BaseParametersViewModel baseParameters)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO BaseParameters(ColorR, ColorG, ColorB, AdminLogin, AdminPassword, [Name]) VALUES({0}, {1}, {2}, '{3}', '{4}', '{5}')",
                    baseParameters.ColorR, baseParameters.ColorG, baseParameters.ColorB, baseParameters.AdminLogin, baseParameters.AdminPassword, baseParameters.Name), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Save(BaseParametersViewModel baseParameters)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("UPDATE BaseParameters SET ColorR = {0}, ColorG = {1}, ColorB = {2}, AdminLogin = '{3}', AdminPassword = '{4}', [Name] = '{5}'",
                    baseParameters.ColorR, baseParameters.ColorG, baseParameters.ColorB, baseParameters.AdminLogin, baseParameters.AdminPassword, baseParameters.Name), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
