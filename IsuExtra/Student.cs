using System;


namespace IsuExtra
{
    public class Student
    {
        public GroupID GroupId;
        public Course Course1;
        public Course Course2;
        public Thread Thread1;
        public Thread Thread2;

        public GroupID StudentsGroup
        {
            get => new GroupID(GroupId);
            private set => GroupId = value;
        }

        public readonly Guid Id = Guid.NewGuid();

        public string Name { get; private set; }

        public void Register(GroupManager manager, Course course, Thread thread)
        {
            if (Course1 == null && Thread1 == null)
            {
                Course1 = course;
                Thread1 = thread;
                //course.AddStudent(this,thread.num);
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

        public void Deregister(MegaFaculty faculty)
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
                if (Course1.Faculty == faculty)
                {
                    Course2 = null;
                    Thread2 = null;
                }
            }
        }

        private Student(string name, GroupID group)
        {
            this.Name = name;
            StudentsGroup = group;
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

            public static implicit operator Student(StudentBuilder builder)
            {
                return builder.Build();
            }
        }
    }
}