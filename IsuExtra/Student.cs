using System;

namespace IsuExtra
{
    public class Student
    {
#pragma warning disable SA1401
        public readonly Guid Id = Guid.NewGuid();
#pragma warning restore SA1401
        private Student(string name, GroupID group)
        {
            this.Name = name;
            StudentsGroup = group;
        }

        public GroupID GroupId { get; private set; }
        public Course Course1 { get; private set; }
        public Course Course2 { get; private set; }
        public Thread Thread1 { get; private set; }
        public Thread Thread2 { get; private set; }

        public GroupID StudentsGroup
        {
            get => new GroupID(GroupId);
            private set => GroupId = value;
        }

        public string Name { get; private set; }

        public void Register(OgnpService manager, Course course, Thread thread)
        {
            if (Course1 == null && Thread1 == null)
            {
                Course1 = course;
                Thread1 = thread;
                thread.AddStudent(this);
                manager.UpdateCourse(course, thread);
            }
            else if (Course2 == null && Thread2 == null)
            {
                Course2 = course;
                Thread2 = thread;
                thread.AddStudent(this);
                manager.UpdateCourse(course, thread);
            }
        }

        public void Deregister(string faculty)
        {
            if (Course1 != null)
            {
                if (Course1.Faculty == faculty)
                {
                    Course1 = null;
                    Thread1 = null;
                }
            }
            else if (Course2 != null)
            {
                if (Course2.Faculty == faculty)
                {
                    Course2 = null;
                    Thread2 = null;
                }
            }
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