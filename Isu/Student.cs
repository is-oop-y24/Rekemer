using System;
using System.Reflection.Metadata.Ecma335;

namespace Isu
{
    public class Student
    {
        private GroupID _studentsGroup;
        private Student(string name, GroupID group)
        {
            this.Name = name;
            StudentsGroup = group;
            Id = GetID();
        }

        public GroupID StudentsGroup
        {
            get => new GroupID(_studentsGroup);
            private set => _studentsGroup = value;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public int GetID()
        {
            return this.GetHashCode();
        }

        public class StudentBuilder
        {
            private string _studentName;
            private GroupID _groupID;
            public static implicit operator Student(StudentBuilder builder)
            {
                return builder.Build();
            }

            public StudentBuilder WithName(string name)
            {
                _studentName = name;
                return this;
            }

            public StudentBuilder ToBuild()
            {
                StudentBuilder studentBuilder = new StudentBuilder();
                return studentBuilder.WithGroup(_groupID).WithName(_studentName);
            }

            public StudentBuilder WithGroup(GroupID id)
            {
                _groupID = id;
                return this;
            }

            public Student Build()
            {
                return new Student(_studentName, _groupID);
            }
        }
    }
}