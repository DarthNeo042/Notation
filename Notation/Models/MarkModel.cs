using Notation.Properties;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public static class MarkModel
    {
        private struct Group
        {
            public int IdClass;
            public int IdPeriod;
            public int IdStudent;
            public int IdSubject;
            public int IdTeacher;
        }

        public static IEnumerable<MarkViewModel> Read(int year, int idPeriod)
        {
            List<MarkViewModel> marks = new List<MarkViewModel>();

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Mark] WHERE [Year] = {year} AND [IdPeriod] = {idPeriod} ORDER BY [Order]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marks.Add(new MarkViewModel()
                            {
                                Id = (int)reader["Id"],
                                Coefficient = (int)(decimal)reader["Coefficient"],
                                Mark = (int)(decimal)reader["Mark"],
                                Order = (int)reader["Order"],
                                IdClass = (int)reader["IdClass"],
                                IdPeriod = (int)reader["IdPeriod"],
                                IdStudent = (int)reader["IdStudent"],
                                IdSubject = (int)reader["IdSubject"],
                                IdTeacher = (int)reader["IdTeacher"],
                                Year = year,
                            });
                        }
                    }
                }
            }

            return marks;
        }

        public static IEnumerable<MarkViewModel> Read(IEnumerable<int> idSubjects, int idStudent, int idTeacher, int idClass, int idPeriod, int year)
        {
            List<MarkViewModel> marks = new List<MarkViewModel>();

            if (idSubjects.Any())
            {
                using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand($"SELECT * FROM [Mark] WHERE IdStudent = {idStudent} AND IdClass = {idClass} AND IdPeriod = {idPeriod}"
                        + $" AND [IdTeacher] = {idTeacher} AND [IdSubject] IN ({string.Join(",", idSubjects)}) AND [Year] = {year} ORDER BY [Order]", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                marks.Add(new MarkViewModel()
                                {
                                    Id = (int)reader["Id"],
                                    Coefficient = (int)(decimal)reader["Coefficient"],
                                    Mark = (int)(decimal)reader["Mark"],
                                    Order = (int)reader["Order"],
                                    IdClass = idClass,
                                    IdPeriod = idPeriod,
                                    IdStudent = idStudent,
                                    IdSubject = (int)reader["IdSubject"],
                                    IdTeacher = idTeacher,
                                    Year = year,
                                });
                            }
                        }
                    }
                }
            }

            return marks;
        }

        public static Dictionary<int, List<MarkViewModel>> Read(IEnumerable<int> idSubjects, IEnumerable<int> idStudents, int idTeacher, int idClass, int idPeriod, int year)
        {
            Dictionary<int, List<MarkViewModel>> marks = new Dictionary<int, List<MarkViewModel>>();

            if (idSubjects.Any() && idStudents.Any())
            {
                using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand($"SELECT * FROM [Mark] WHERE IdStudent IN ({string.Join(", ", idStudents)}) AND IdClass = {idClass} AND IdPeriod = {idPeriod}"
                        + $" AND [IdTeacher] = {idTeacher} AND [IdSubject] IN ({string.Join(",", idSubjects)}) AND [Year] = {year} ORDER BY [Order]", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!marks.ContainsKey((int)reader["idStudent"]))
                                {
                                    marks[(int)reader["idStudent"]] = new List<MarkViewModel>();
                                }
                                marks[(int)reader["idStudent"]].Add(new MarkViewModel()
                                {
                                    Id = (int)reader["Id"],
                                    Coefficient = (int)(decimal)reader["Coefficient"],
                                    Mark = (int)(decimal)reader["Mark"],
                                    Order = (int)reader["Order"],
                                    IdClass = idClass,
                                    IdPeriod = idPeriod,
                                    IdStudent = (int)reader["idStudent"],
                                    IdSubject = (int)reader["IdSubject"],
                                    IdTeacher = idTeacher,
                                    Year = year,
                                });
                            }
                        }
                    }
                }
            }

            foreach (int idStudent in idStudents.Where(s => !marks.ContainsKey(s)))
            {
                marks[idStudent] = new List<MarkViewModel>();
            }

            return marks;
        }

        public static TeacherViewModel ReadTeacherFromClassAndSubject(int year, int idClass, int idSubject)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT TOP 1 IdTeacher FROM Mark WHERE [Year] = {year} AND IdClass = {idClass} AND IdSubject = {idSubject}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.Id == (int)reader["IdTeacher"]);
                        }
                    }
                }
            }

            return null;
        }

        public static void Save(IEnumerable<MarkViewModel> marks, int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                foreach (IGrouping<Group, MarkViewModel> markGroup in marks.GroupBy(m => new Group() { IdClass = m.IdClass, IdPeriod = m.IdPeriod, IdStudent = m.IdStudent, IdSubject = m.IdSubject, IdTeacher = m.IdTeacher }))
                {
                    using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM Mark WHERE [Year] = {0} AND IdClass = {1} AND IdPeriod = {2} AND IdStudent = {3} AND IdSubject = {4} AND IdTeacher = {5}",
                        year, markGroup.Key.IdClass, markGroup.Key.IdPeriod, markGroup.Key.IdStudent, markGroup.Key.IdSubject, markGroup.Key.IdTeacher), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    foreach (MarkViewModel Mark in markGroup)
                    {
                        using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO Mark([Year], Mark, Coefficient, [Order], IdClass, IdPeriod, IdStudent, IdSubject, IdTeacher)"
                            + " VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                            year, Mark.Mark, Mark.Coefficient, Mark.Order, Mark.IdClass, Mark.IdPeriod, Mark.IdStudent, Mark.IdSubject, Mark.IdTeacher), connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void DeleteAll(int year)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"DELETE FROM Mark WHERE Year = {year}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static double ReadPeriodSubjectAverage(PeriodViewModel period, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " WHERE Mark.[Year] = {0} AND IdPeriod = {1} AND IdStudent = {2} AND IdSubject = {3}", period.Year, period.Id, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadPeriodMainSubjectAverage(PeriodViewModel period, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " WHERE Mark.[Year] = {0} AND IdPeriod = {1} AND IdStudent = {2} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + " WHERE Subject.[Year] = {0} AND Subject.ParentSubjectId = {3}", period.Year, period.Id, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadPeriodTrimesterSubjectAverage(PeriodViewModel period, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod INNER JOIN Period Period2"
                    + " ON Period2.[Year] = Mark.[Year] AND Period2.Trimester = Period.Trimester AND Period.Number <= Period2.Number"
                    + " WHERE Mark.[Year] = {0} AND Period2.Id = {1} AND IdStudent = {2} AND IdSubject = {3}", period.Year, period.Id, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadPeriodTrimesterAverage(PeriodViewModel period, StudentViewModel student)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod INNER JOIN Period Period2"
                    + " ON Period2.[Year] = Mark.[Year] AND Period2.Trimester = Period.Trimester AND Period2.Number <= Period.Number"
                    + " WHERE Mark.[Year] = {0} AND IdPeriod = {1} AND IdStudent = {2} GROUP BY IdSubject) SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage",
                    period.Year, period.Id, student.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadSemiTrimesterSubjectAverage(SemiTrimesterViewModel semiTrimester, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + " WHERE Mark.[Year] = {0} AND SemiTrimester.Id = {1} AND IdStudent = {2} AND IdSubject = {3}", semiTrimester.Year, semiTrimester.Id, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadSemiTrimesterAverage(SemiTrimesterViewModel semiTrimester, StudentViewModel student)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + " WHERE Mark.[Year] = {0} AND SemiTrimester.Id = {1} AND IdStudent = {2} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", semiTrimester.Year, semiTrimester.Id, student.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadSemiTrimesterClassSubjectAverage(SemiTrimesterViewModel semiTrimester, ClassViewModel _class, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + " WHERE Mark.[Year] = {0} AND SemiTrimester.Id = {1} AND IdClass = {2} AND IdSubject = {3}", semiTrimester.Year, semiTrimester.Id, _class.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadSemiTrimesterClassAverage(SemiTrimesterViewModel semiTrimester, ClassViewModel _class)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + " WHERE Mark.[Year] = {0} AND SemiTrimester.Id = {1} AND IdClass = {2} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", semiTrimester.Year, semiTrimester.Id, _class.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadSemiTrimesterMainSubjectAverage(SemiTrimesterViewModel semiTrimester, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + " WHERE Mark.[Year] = {0} AND SemiTrimester.Id = {1} AND IdStudent = {2} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + " WHERE Subject.[Year] = {0} AND Subject.ParentSubjectId = {3}", semiTrimester.Year, semiTrimester.Id, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadSemiTrimesterClassMainSubjectAverage(SemiTrimesterViewModel semiTrimester, ClassViewModel _class, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + " WHERE Mark.[Year] = {0} AND SemiTrimester.Id = {1} AND IdClass = {2} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + " WHERE Subject.[Year] = {0} AND Subject.ParentSubjectId = {3}", semiTrimester.Year, semiTrimester.Id, _class.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static void ReadSemiTrimesterMinMaxSubjectAverage(SemiTrimesterViewModel semiTrimester, ClassViewModel _class, SubjectViewModel subject, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadSemiTrimesterSubjectAverage(semiTrimester, student, subject);
                if (average != double.MinValue)
                {
                    if (average < minAverage)
                    {
                        minAverage = average;
                    }
                    if (average > maxAverage)
                    {
                        maxAverage = average;
                    }
                }
            }
        }

        public static void ReadSemiTrimesterMinMaxMainSubjectAverage(SemiTrimesterViewModel semiTrimester, ClassViewModel _class, SubjectViewModel subject, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadSemiTrimesterMainSubjectAverage(semiTrimester, student, subject);
                if (average != double.MinValue)
                {
                    if (average < minAverage)
                    {
                        minAverage = average;
                    }
                    if (average > maxAverage)
                    {
                        maxAverage = average;
                    }
                }
            }
        }

        public static void ReadSemiTrimesterMinMaxAverage(SemiTrimesterViewModel semiTrimester, ClassViewModel _class, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadSemiTrimesterAverage(semiTrimester, student);
                if (average != double.MinValue)
                {
                    if (average < minAverage)
                    {
                        minAverage = average;
                    }
                    if (average > maxAverage)
                    {
                        maxAverage = average;
                    }
                }
            }
        }

        public static int ReadSemiTrimesterRanking(StudentViewModel student, Dictionary<StudentViewModel, double> averages)
        {
            double average = averages[student];
            return averages.Values.OrderByDescending(v => v).ToList().IndexOf(average);
        }

        public static double ReadTrimesterSubjectAverage(int trimester, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + " WHERE Mark.[Year] = {0} AND Period.Trimester = {1} AND IdStudent = {2} AND IdSubject = {3}", student.Year, trimester, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadTrimesterAverage(int trimester, StudentViewModel student)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + " WHERE Mark.[Year] = {0} AND Period.Trimester = {1} AND IdStudent = {2} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", student.Year, trimester, student.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadTrimesterClassSubjectAverage(int trimester, ClassViewModel _class, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + " WHERE Mark.[Year] = {0} AND Period.Trimester = {1} AND IdClass = {2} AND IdSubject = {3}", _class.Year, trimester, _class.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadTrimesterClassAverage(int trimester, ClassViewModel _class)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + " WHERE Mark.[Year] = {0} AND Period.Trimester = {1} AND IdClass = {2} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", _class.Year, trimester, _class.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadTrimesterMainSubjectAverage(int trimester, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + " WHERE Mark.[Year] = {0} AND Period.Trimester = {1} AND IdStudent = {2} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + " WHERE Subject.[Year] = {0} AND Subject.ParentSubjectId = {3}", student.Year, trimester, student.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static double ReadTrimesterClassMainSubjectAverage(int trimester, ClassViewModel _class, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(string.Format("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + " WHERE Mark.[Year] = {0} AND Period.Trimester = {1} AND IdClass = {2} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + " WHERE Subject.[Year] = {0} AND Subject.ParentSubjectId = {3}", _class.Year, trimester, _class.Id, subject.Id), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            average = reader.IsDBNull(reader.GetOrdinal("Average")) ? double.MinValue : (double)(decimal)reader["Average"];
                        }
                    }
                }
            }

            return average;
        }

        public static void ReadTrimesterMinMaxSubjectAverage(int trimester, ClassViewModel _class, SubjectViewModel subject, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadTrimesterSubjectAverage(trimester, student, subject);
                if (average != double.MinValue)
                {
                    if (average < minAverage)
                    {
                        minAverage = average;
                    }
                    if (average > maxAverage)
                    {
                        maxAverage = average;
                    }
                }
            }
        }

        public static void ReadTrimesterMinMaxMainSubjectAverage(int trimester, ClassViewModel _class, SubjectViewModel subject, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadTrimesterMainSubjectAverage(trimester, student, subject);
                if (average != double.MinValue)
                {
                    if (average < minAverage)
                    {
                        minAverage = average;
                    }
                    if (average > maxAverage)
                    {
                        maxAverage = average;
                    }
                }
            }
        }

        public static void ReadTrimesterMinMaxAverage(int trimester, ClassViewModel _class, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadTrimesterAverage(trimester, student);
                if (average != double.MinValue)
                {
                    if (average < minAverage)
                    {
                        minAverage = average;
                    }
                    if (average > maxAverage)
                    {
                        maxAverage = average;
                    }
                }
            }
        }

        public static int ReadtrimesterRanking(StudentViewModel student, Dictionary<StudentViewModel, double> averages)
        {
            double average = averages[student];
            return averages.Values.OrderByDescending(v => v).ToList().IndexOf(average);
        }
    }
}
