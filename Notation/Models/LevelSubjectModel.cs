using Notation.Properties;
using Notation.ViewModels;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class LevelSubjectModel
    {
        public static void ReadLevelSubjects(ObservableCollection<LevelViewModel> levels, ObservableCollection<SubjectViewModel> subjects, int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [LevelSubject] WHERE Year = {0}", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idLevel = (int)reader["IdLevel"];
                            LevelViewModel level = levels.FirstOrDefault(l => l.Id == idLevel);
                            if (level != null)
                            {
                                int idSubject = (int)reader["IdSubject"];
                                SubjectViewModel subject = subjects.FirstOrDefault(s => s.Id == idSubject);
                                if (subject != null)
                                {
                                    level.Subjects.Add(subject);
                                    subject.Levels.Add(level);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveLevelSubjects(LevelViewModel level)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM [LevelSubject] WHERE [Year] = {0} AND IdLevel = {1}", level.Year, level.Id), connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (SubjectViewModel subject in level.Subjects)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [LevelSubject]([Year], IdLevel, IdSubject) VALUES({0}, {1}, {2})", level.Year, level.Id, subject.Id), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveSubjectLevels(SubjectViewModel subject)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM [LevelSubject] WHERE [Year] = {0} AND IdSubject = {1}", subject.Year, subject.Id), connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (LevelViewModel level in subject.Levels)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [LevelSubject]([Year], IdSubject, IdLevel) VALUES({0}, {1}, {2})", subject.Year, subject.Id, level.Id), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM LevelSubject WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
