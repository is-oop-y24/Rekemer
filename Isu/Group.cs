using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
#pragma warning disable SA1401
        public readonly int MaxStudents;
#pragma warning restore SA1401

        private GroupID _groupInfo;
        private Group(GroupID id, int maxStudents, List<Student> students)
        {
            GroupInfo = id;
            this.MaxStudents = maxStudents;
            _students = students;
        }

        public GroupID GroupInfo
        {
            get => new GroupID(_groupInfo);
            private set => _groupInfo = value;
        }
#pragma warning disable SA1201
        private List<Student> _students;
#pragma warning restore SA1201

        public List<Student> Students => new List<Student>(_students);

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
            private GroupID _groupID;
            private List<Student> _students;
            public static implicit operator Group(GroupBuilder builder)
            {
                return builder.Build();
            }

            public GroupBuilder WithName(string name)
            {
                _groupID = GroupID.ParseName(name);
                return this;
            }

            public GroupBuilder WithMaxAmountOfStudents(int max)
            {
                _maxStudents = max;
                return this;
            }

            public GroupBuilder WithStudents(params Student[] students)
            {
                _students = students.ToList();
                return this;
            }

            public Group Build()
            {
                return new Group(_groupID, _maxStudents, _students ?? new List<Student>());
            }

            public GroupBuilder ToBuild()
            {
                GroupBuilder studentBuilder = new GroupBuilder();
                return studentBuilder.WithStudents(_students.ToArray()).WithMaxAmountOfStudents(_maxStudents).WithName(_groupID);
            }
        }
    }
}