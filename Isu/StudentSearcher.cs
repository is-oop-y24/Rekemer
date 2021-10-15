using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    class StudentSearcher
    {
        public Student FindById(int id, GroupManager manager)
        {
            Student student = null;
            student = FindByParametr(id, student, manager);

            return student;
        }

        private Student FindByParametr<T>(T id, Student student, GroupManager manager)
        {
            for (int i = 0; i < manager.dataOfGroupes.Length; i++)
            {
                if (manager.dataOfGroupes[i] == null) continue;
                for (int j = 0; j < manager.dataOfGroupes[i].Count; j++)
                {
                    var groups = manager.dataOfGroupes[i];
                    for (int k = 0; k < groups.Count; k++)
                    {
                        student = groups[k].students
                            .FirstOrDefault(t => (id is int ? Equals(t.id, id) : Equals(t.name, id)));
                    }
                }
            }

            return student;
        }

        public Student FindByName(string name, GroupManager manager)
        {
            Student student = null;
            student = FindByParametr(name, student, manager);

            return student;
        }

        public List<Student> GetStudentsOfgroup(string nameOfGroup, GroupManager manager)
        {
            GroupID groupId = GroupID.ParseName(nameOfGroup);
            Group group = manager.dataOfGroupes[groupId.courseNum].
                FirstOrDefault(t => t.GroupInfo.Name == nameOfGroup);
            if (group == null) throw new IsuException($"{nameOfGroup} is empty");
            return group.students;
        }

        public List<Student> GetStudentsOfCourse(CourseNumber courseNumber, GroupManager manager)
        {
            List<Student> students = new List<Student>();
            var currCourse = manager.dataOfGroupes[courseNumber];
            foreach (Group tGroup in currCourse)
            {
                students.AddRange(tGroup.students);
            }

            if (students.Count == 0) throw new IsuException($"There are no students on this {courseNumber} course");
            return students;
        }
    }
}