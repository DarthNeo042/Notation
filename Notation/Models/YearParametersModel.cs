using Notation.Settings;
using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class YearParametersModel
    {
        public static YearParametersViewModel Read(int year)
        {
            YearParametersViewModel yearParameters = null;

            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [YearParameters] WHERE Year = {year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            yearParameters = new YearParametersViewModel()
                            {
                                DivisionPrefect = (string)reader["DivisionPrefect"],
                                Id = (int)reader["Id"],
                                Year = (int)reader["Year"],
                            };
                        }
                    }
                }
            }

            return yearParameters;
        }

        public static void Create(YearParametersViewModel yearParameters)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"INSERT INTO YearParameters(Year, DivisionPrefect) VALUES({yearParameters.Year}, '{yearParameters.DivisionPrefect}')", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Save(YearParametersViewModel yearParameters)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"UPDATE YearParameters SET DivisionPrefect = '{yearParameters.DivisionPrefect.Replace("'", "''")}' WHERE Id = {yearParameters.Id} AND Year = {yearParameters.Year}", connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM YearParameters WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
