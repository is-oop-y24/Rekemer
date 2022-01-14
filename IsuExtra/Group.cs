using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace IsuExtra
{
    public class Group
    {
#pragma warning disable SA1401
        public readonly int MaxStudents;
#pragma warning restore SA1401

#pragma warning disable SA1401
        public List<Lesson> Lessons = new List<Lesson>();
#pragma warning restore SA1401
        private readonly List<Student> _students;

        private Group(GroupID id, int maxStudents, List<Student> students)
        {
            GroupInfo = id;
            this.MaxStudents = maxStudents;
            _students = students;
        }

        public GroupID GroupInfo { get; private set; }

        public List<Student> Students => new List<Student>(_students);

        public void AddLessons(List<Lesson> lessons)
        {
            Lessons = Lessons.Union(lessons).ToList();
        }

        public void AddLesson(Lesson lesson)
        {
            Lessons.Add(lesson);
        }

        public void AddStudent(Student student)
        {
            if (_students.Count == MaxStudents)
                throw new IsuException($"Student can not be added in the group {GroupInfo.Name}, the group is full");
            string name = student.Name;
            bool isThereAlreadyTheSameStudent = _students.Any(t => t.Name == name);
            if (isThereAlreadyTheSameStudent) throw new IsuException($"Student already in {GroupInfo.Name}");

            _students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            foreach (Student tstudent in Students)
            {
                if (tstudent.Id != student.Id) continue;
                Students.Remove(tstudent);
                break;
            }
        }

        public class GroupBuilder
        {
            private int _maxStudents;
            private GroupID _groupId;
            private List<Student> _students;

            public static implicit operator Group(GroupBuilder builder)
            {
                return builder.Build();
            }

            public GroupBuilder WithName(string name)
            {
                _groupId = GroupID.ParseName(name);
                return this;
            }

            public GroupBuilder WithMaxAmountOfStudents(int max)
            {
                _maxStudents = max;
                return this;
            }

            public GroupBuilder WithStudents(List<Student> students)
            {
                _students = students;
                return this;
            }

            public Group Build()
            {
                return new Group(_groupId, _maxStudents, _students ?? new List<Student>());
            }

            public GroupBuilder ToBuild()
            {
                GroupBuilder studentBuilder = new GroupBuilder();
                return studentBuilder.WithStudents(_students).WithMaxAmountOfStudents(_maxStudents).WithName(_groupId);
            }
        }
    }
}