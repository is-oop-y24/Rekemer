using System;

namespace Isu
{
    public class Student
    {
        public GroupID studentsGroup { get; private set; }
        public int id { get; private set; }
        public string name { get; private set; }


        private Student()
        {
            
        }

        private Student(string name, GroupID group)
        {
            this.name = name;
            studentsGroup = group;
            id = GetID();
        }

        public int GetID()
        {
            return this.GetHashCode();
        }
        
        public class StudentBuilder
        {
            private string _studentName;
            private GroupID _groupID;
            public StudentBuilder WithName(string name)
            {
                _studentName = name;
                return this;
            
            }
            public StudentBuilder WithGroup(GroupID id)
            {
                _groupID = id;
                return this;
            }

            public Student Build()
            {
                return new Student(_studentName,_groupID);
            }
            public static implicit operator Student(StudentBuilder builder)
            {
                return builder.Build();
            }
        }
    }
}