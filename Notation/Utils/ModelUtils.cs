using Notation.Models;
using Notation.ViewModels;
using System.Linq;

namespace Notation.Utils
{
    public static class ModelUtils
    {
        public static TeacherViewModel GetTeacherFromClassAndSubject(ClassViewModel _class, SubjectViewModel subject)
        {
            TeacherViewModel teacher = MarkModel.ReadTeacherFromClassAndSubject(_class.Year, _class.Id, subject.Id);

            if (teacher == null)
            {
                teacher = MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.Classes.Any(c => c.Id == _class.Id) && t.Subjects.Any(s => s.Id == subject.Id));
            }

            return teacher;
        }

        public static PeriodViewModel GetPreviousPeriod(PeriodViewModel period)
        {
            return MainViewModel.Instance.Parameters.Periods.Where(p => p.Trimester == period.Trimester && p.Number < period.Number).OrderByDescending(p => p.Number).FirstOrDefault();
        }
    }
}
