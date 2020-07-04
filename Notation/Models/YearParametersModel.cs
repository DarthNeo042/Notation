using Notation.ViewModels;
using System.Data.SqlClient;

namespace Notation.Models
{
    public static class YearParametersModel
    {
        public static YearParametersViewModel Read(int year)
        {
            YearParametersViewModel yearParameters = null;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [YearParameters] WHERE Year = {0}", year), connection))
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
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO YearParameters(Year, DivisionPrefect) VALUES({0}, '{1}')",
                    yearParameters.Year, yearParameters.DivisionPrefect), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Save(YearParametersViewModel yearParameters)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("UPDATE YearParameters SET DivisionPrefect = '{0}' WHERE Id = {1} AND Year = {2}",
                    yearParameters.DivisionPrefect, yearParameters.Id, yearParameters.Year), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
