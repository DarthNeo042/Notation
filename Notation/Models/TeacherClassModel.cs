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
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT * FROM [TeacherClass] WHERE Year = {0}", year), connection))
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
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM [TeacherClass] WHERE [Year] = {0} AND IdTeacher = {1}", teacher.Year, teacher.Id), connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (ClassViewModel _class in teacher.Classes)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [TeacherClass]([Year], IdTeacher, IdClass) VALUES({0}, {1}, {2})", teacher.Year, teacher.Id, _class.Id), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveClassTeachers(ClassViewModel _class)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM [TeacherClass] WHERE [Year] = {0} AND IdClass = {1}", _class.Year, _class.Id), connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (TeacherViewModel teacher in _class.Teachers)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO [TeacherClass]([Year], IdClass, IdTeacher) VALUES({0}, {1}, {2})", _class.Year, _class.Id, teacher.Id), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
