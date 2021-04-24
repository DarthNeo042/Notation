using Notation.Properties;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Notation.Models
{
    public class MarkModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public decimal Mark { get; set; }
        public decimal Coefficient { get; set; }
        public int Order { get; set; }
        public int IdClass { get; set; }
        public int IdPeriod { get; set; }
        public int IdSubject { get; set; }
        public int IdStudent { get; set; }
        public int IdTeacher { get; set; }

        private struct Group
        {
            public int IdClass;
            public int IdPeriod;
            public int IdStudent;
            public int IdSubject;
            public int IdTeacher;
        }

        public static IEnumerable<MarkModel> Read(int year, int idPeriod)
        {
            List<MarkModel> marks = new List<MarkModel>();

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Mark] WHERE [Year] = {year} AND [IdPeriod] = {idPeriod} ORDER BY [Order]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marks.Add(new MarkModel()
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

        public static IEnumerable<MarkModel> Read(IEnumerable<int> idSubjects, int idStudent, int idTeacher, int idClass, int idPeriod, int year)
        {
            List<MarkModel> marks = new List<MarkModel>();

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
                                marks.Add(new MarkModel()
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

        public static Dictionary<int, List<MarkModel>> Read(IEnumerable<int> idSubjects, IEnumerable<int> idStudents, int idTeacher, int idClass, int idPeriod, int year)
        {
            Dictionary<int, List<MarkModel>> marks = new Dictionary<int, List<MarkModel>>();

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
                                    marks[(int)reader["idStudent"]] = new List<MarkModel>();
                                }
                                marks[(int)reader["idStudent"]].Add(new MarkModel()
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
                marks[idStudent] = new List<MarkModel>();
            }

            return marks;
        }

        public static TeacherViewModel ReadTeacherFromClassAndSubject(int year, int idClass, int idSubject, int idPeriod)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT TOP 1 IdTeacher FROM Mark WHERE [Year] = {year} AND IdClass = {idClass} AND IdSubject = {idSubject} AND IdPeriod = {idPeriod}", connection))
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

        public static void Save(IEnumerable<MarkModel> marks, int year, bool replaceTeacher = false)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                foreach (IGrouping<Group, MarkModel> markGroup in marks.GroupBy(m => new Group() { IdClass = m.IdClass, IdPeriod = m.IdPeriod, IdStudent = m.IdStudent, IdSubject = m.IdSubject, IdTeacher = m.IdTeacher }))
                {
                    using (SqlCommand command = new SqlCommand($"DELETE FROM Mark WHERE [Year] = {year} AND IdClass = {markGroup.Key.IdClass} AND IdPeriod = {markGroup.Key.IdPeriod}"
                        + $" AND IdStudent = {markGroup.Key.IdStudent} AND IdSubject = {markGroup.Key.IdSubject}{(!replaceTeacher ? $" AND IdTeacher = {markGroup.Key.IdTeacher}" : "")}", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    foreach (MarkModel mark in markGroup)
                    {
                        using (SqlCommand command = new SqlCommand("INSERT INTO Mark([Year], Mark, Coefficient, [Order], IdClass, IdPeriod, IdStudent, IdSubject, IdTeacher)"
                            + $" VALUES({year}, {mark.Mark}, {mark.Coefficient}, {mark.Order}, {mark.IdClass}, {mark.IdPeriod}, {mark.IdStudent}, {mark.IdSubject}, {mark.IdTeacher})", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {period.Year} AND IdPeriod = {period.Id} AND IdStudent = {student.Id} AND IdSubject = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {period.Year} AND IdPeriod = {period.Id} AND IdStudent = {student.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {period.Year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod INNER JOIN Period Period2"
                    + " ON Period2.[Year] = Mark.[Year] AND Period2.Trimester = Period.Trimester AND Period.Number <= Period2.Number"
                    + $" WHERE Mark.[Year] = {period.Year} AND Period2.Id = {period.Id} AND IdStudent = {student.Id} AND IdSubject = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod INNER JOIN Period Period2"
                    + " ON Period2.[Year] = Mark.[Year] AND Period2.Trimester = Period.Trimester AND Period.Number <= Period2.Number"
                    + $" WHERE Mark.[Year] = {period.Year} AND Period2.Id = {period.Id} AND IdStudent = {student.Id} GROUP BY IdSubject) SubjectAverage"
                    + " ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + $" WHERE Mark.[Year] = {semiTrimester.Year} AND SemiTrimester.Id = {semiTrimester.Id} AND IdStudent = {student.Id} AND IdSubject = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + $" WHERE Mark.[Year] = {semiTrimester.Year} AND SemiTrimester.Id = {semiTrimester.Id} AND IdStudent = {student.Id} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + $" WHERE Mark.[Year] = {semiTrimester.Year} AND SemiTrimester.Id = {semiTrimester.Id} AND IdClass = {_class.Id} AND IdSubject = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + $" WHERE Mark.[Year] = {semiTrimester.Year} AND SemiTrimester.Id = {semiTrimester.Id} AND IdClass = {_class.Id} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + $" WHERE Mark.[Year] = {semiTrimester.Year} AND SemiTrimester.Id = {semiTrimester.Id} AND IdStudent = {student.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {semiTrimester.Year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN SemiTrimester ON SemiTrimester.[Year] = Mark.[Year] AND(SemiTrimester.IdPeriod1 = Mark.IdPeriod OR SemiTrimester.IdPeriod2 = Mark.IdPeriod)"
                    + $" WHERE Mark.[Year] = {semiTrimester.Year} AND SemiTrimester.Id = {semiTrimester.Id} AND IdClass = {_class.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {semiTrimester.Year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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
            return averages.Values.OrderByDescending(v => v).ToList().IndexOf(average) + 1;
        }

        public static double ReadTrimesterSubjectAverage(int trimester, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + $" WHERE Mark.[Year] = {student.Year} AND Period.Trimester = {trimester} AND IdStudent = {student.Id} AND IdSubject = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + $" WHERE Mark.[Year] = {student.Year} AND Period.Trimester = {trimester} AND IdStudent = {student.Id} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + $" WHERE Mark.[Year] = {_class.Year} AND Period.Trimester = {trimester} AND IdClass = {_class.Id} AND IdSubject = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + $" WHERE Mark.[Year] = {_class.Year} AND Period.Trimester = {trimester} AND IdClass = {_class.Id} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + $" WHERE Mark.[Year] = {student.Year} AND Period.Trimester = {trimester} AND IdStudent = {student.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {student.Year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + " INNER JOIN Period ON Period.[Year] = Mark.[Year] AND Period.Id = Mark.IdPeriod"
                    + $" WHERE Mark.[Year] = {_class.Year} AND Period.Trimester = {trimester} AND IdClass = {_class.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {_class.Year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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
            return averages.Values.OrderByDescending(v => v).ToList().IndexOf(average) + 1;
        }

        public static double ReadYearSubjectAverage(int year, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {year} AND IdStudent = {student.Id} AND IdSubject = {subject.Id}", connection))
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

        public static double ReadYearAverage(int year, StudentViewModel student)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {year} AND IdStudent = {student.Id} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

        public static double ReadYearClassSubjectAverage(int year, ClassViewModel _class, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {year} AND IdClass = {_class.Id} AND IdSubject = {subject.Id}", connection))
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

        public static double ReadYearClassAverage(int year, ClassViewModel _class)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(CoefficientAverage) / SUM(Coefficient), 1) AS Average"
                    + " FROM(SELECT CASE WHEN Subject.[Option] = 1 THEN CASE WHEN SubjectAverage.Average > 10 THEN(SubjectAverage.Average - 10) * Subject.Coefficient ELSE 0 END"
                    + " ELSE SubjectAverage.Average * Subject.Coefficient END AS[CoefficientAverage], CASE WHEN Subject.[Option] = 1 THEN 0 ELSE Subject.Coefficient END AS Coefficient"
                    + " FROM Subject INNER JOIN(SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {year} AND IdClass = {_class.Id} GROUP BY IdSubject)"
                    + " SubjectAverage ON SubjectAverage.IdSubject = Subject.Id) AS SubjectCoefficientAverage", connection))
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

        public static double ReadYearMainSubjectAverage(int year, StudentViewModel student, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {year} AND IdStudent = {student.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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

        public static double ReadYearClassMainSubjectAverage(int year, ClassViewModel _class, SubjectViewModel subject)
        {
            double average = double.MinValue;

            using (SqlConnection connection = new SqlConnection(Settings.Default.SQLConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT ROUND(SUM(Coefficient * Average) / SUM(Coefficient), 1) AS Average FROM"
                    + " (SELECT IdSubject, ROUND(SUM(Coefficient * Mark) / SUM(Coefficient), 1) AS Average FROM Mark"
                    + $" WHERE Mark.[Year] = {year} AND IdClass = {_class.Id} GROUP BY IdSubject) AS SubjectAverage INNER JOIN Subject ON Subject.Id = SubjectAverage.IdSubject"
                    + $" WHERE Subject.[Year] = {year} AND Subject.ParentSubjectId = {subject.Id}", connection))
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

        public static void ReadYearMinMaxSubjectAverage(int year, ClassViewModel _class, SubjectViewModel subject, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadYearSubjectAverage(year, student, subject);
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

        public static void ReadYearMinMaxMainSubjectAverage(int year, ClassViewModel _class, SubjectViewModel subject, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadYearMainSubjectAverage(year, student, subject);
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

        public static void ReadYearMinMaxAverage(int year, ClassViewModel _class, out double minAverage, out double maxAverage)
        {
            minAverage = double.MaxValue;
            maxAverage = double.MinValue;
            foreach (StudentViewModel student in _class.Students)
            {
                double average = ReadYearAverage(year, student);
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

        public static int ReadYearRanking(StudentViewModel student, Dictionary<StudentViewModel, double> averages)
        {
            double average = averages[student];
            return averages.Values.OrderByDescending(v => v).ToList().IndexOf(average) + 1;
        }
    }
}
