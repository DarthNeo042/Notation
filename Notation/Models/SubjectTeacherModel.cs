using Notation.Properties;
using Notation.ViewModels;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class SubjectTeacherModel
    {
        public static void ReadSubjectTeachers(ObservableCollection<SubjectViewModel> subjects, ObservableCollection<TeacherViewModel> teachers, int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [SubjectTeacher] WHERE Year = {0}", year), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idSubject = (int)reader["IdSubject"];
                            SubjectViewModel subject = subjects.FirstOrDefault(s => s.Id == idSubject);
                            if (subject != null)
                            {
                                int idTeacher = (int)reader["IdTeacher"];
                                TeacherViewModel teacher = teachers.FirstOrDefault(t => t.Id == idTeacher);
                                if (teacher != null)
                                {
                                    subject.Teachers.Add(teacher);
                                    teacher.Subjects.Add(subject);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveSubjectTeachers(SubjectViewModel subject)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM [SubjectTeacher] WHERE [Year] = {0} AND IdSubject = {1}", subject.Year, subject.Id), connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (TeacherViewModel teacher in subject.Teachers)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [SubjectTeacher]([Year], IdSubject, IdTeacher) VALUES({0}, {1}, {2})", subject.Year, subject.Id, teacher.Id), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveTeacherSubjects(TeacherViewModel teacher)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM [SubjectTeacher] WHERE [Year] = {0} AND IdTeacher = {1}", teacher.Year, teacher.Id), connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (SubjectViewModel subject in teacher.Subjects)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [SubjectTeacher]([Year], IdTeacher, IdSubject) VALUES({0}, {1}, {2})", teacher.Year, teacher.Id, subject.Id), connection))
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
                using (SqlCommand command = new SqlCommand($"DELETE FROM SubjectTeacher WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
