using Notation.Settings;
using Notation.ViewModels;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class TeacherClassModel
    {
        public static void ReadTeacherClasss(ObservableCollection<TeacherViewModel> teachers, ObservableCollection<ClassViewModel> classes, int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [TeacherClass] WHERE Year = {year}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idTeacher = (int)reader["IdTeacher"];
                            TeacherViewModel teacher = teachers.FirstOrDefault(t => t.Id == idTeacher);
                            if (teacher != null)
                            {
                                int idClass = (int)reader["IdClass"];
                                ClassViewModel _class = classes.FirstOrDefault(c => c.Id == idClass);
                                if (_class != null)
                                {
                                    teacher.Classes.Add(_class);
                                    _class.Teachers.Add(teacher);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SaveTeacherClasses(TeacherViewModel teacher)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE FROM [TeacherClass] WHERE [Year] = {teacher.Year} AND IdTeacher = {teacher.Id}", connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (ClassViewModel _class in teacher.Classes)
                {
                    using (SqlCommand command = new SqlCommand($"INSERT INTO [TeacherClass]([Year], IdTeacher, IdClass) VALUES({teacher.Year}, {teacher.Id}, {_class.Id})", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveClassTeachers(ClassViewModel _class)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"DELETE FROM [TeacherClass] WHERE [Year] = {_class.Year} AND IdClass = {_class.Id}", connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (TeacherViewModel teacher in _class.Teachers)
                {
                    using (SqlCommand command = new SqlCommand($"INSERT INTO [TeacherClass]([Year], IdClass, IdTeacher) VALUES({_class.Year}, {_class.Id}, {teacher.Id})", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Settings.Instance.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM TeacherClass WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
