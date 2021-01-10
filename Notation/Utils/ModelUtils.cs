using Notation.Models;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Notation.Utils
{
    public static class ModelUtils
    {
        public static TeacherViewModel GetTeacherFromClassAndSubject(ClassViewModel _class, SubjectViewModel subject, PeriodViewModel period)
        {
            TeacherViewModel teacher = MarkModel.ReadTeacherFromClassAndSubject(_class.Year, _class.Id, subject.Id, period.Id);

            if (teacher == null)
            {
                teacher = MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.Classes.Any(c => c.Id == _class.Id) && t.Subjects.Any(s => s.Id == subject.Id));
            }

            return teacher;
        }

        public static IEnumerable<TeacherViewModel> GetTeachersFromClassAndSubject(ClassViewModel _class, SubjectViewModel subject, PeriodViewModel period)
        {
            TeacherViewModel teacher = MarkModel.ReadTeacherFromClassAndSubject(_class.Year, _class.Id, subject.Id, period.Id);

            if (teacher == null)
            {
                return MainViewModel.Instance.Parameters.Teachers.Where(t => t.Classes.Any(c => c.Id == _class.Id) && t.Subjects.Any(s => s.Id == subject.Id));
            }

            return new List<TeacherViewModel>() { teacher };
        }

        public static PeriodViewModel GetPreviousPeriod(PeriodViewModel period)
        {
            return MainViewModel.Instance.Parameters.Periods.Where(p => p.Trimester == period.Trimester && p.Number < period.Number).OrderByDescending(p => p.Number).FirstOrDefault();
        }
    }
}
