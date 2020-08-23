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

                using (SqlCommand command = new SqlCommand("INSERT INTO BaseParameters(ColorR, ColorG, ColorB, AdminLogin, AdminPassword, [Name])"
                    + $" VALUES({baseParameters.ColorR}, {baseParameters.ColorG}, {baseParameters.ColorB}, '{baseParameters.AdminLogin}', '{baseParameters.AdminPassword}', '{baseParameters.Name}')", connection))
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

                using (SqlCommand command = new SqlCommand($"UPDATE BaseParameters SET ColorR = {baseParameters.ColorR}, ColorG = {baseParameters.ColorG}, ColorB = {baseParameters.ColorB},"
                    + $" AdminLogin = '{baseParameters.AdminLogin}', AdminPassword = '{baseParameters.AdminPassword}', [Name] = '{baseParameters.Name}'", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
