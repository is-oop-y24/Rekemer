using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        public readonly int maxStudents;

        private GroupID _groupInfo;
        public GroupID GroupInfo
        {
            get=> new GroupID(_groupInfo);
            private set => _groupInfo = value;
        }
        
        private List<Student> _students;

        public List<Student> students => new List<Student>(_students);

        private Group(GroupID id, int maxStudents, List<Student> students)
        {
            GroupInfo =  id;
            this.maxStudents = maxStudents;
            _students = students;
        }
        public void AddStudent(Student student)
        {
            if (_students.Count == maxStudents)
                throw new IsuException($"Student can not be added in the group {GroupInfo.Name}, the group is full");
            string name = student.name;
            bool isThereAlreadyTheSameStudent = _students.Any(t => t.name == name);
            if (isThereAlreadyTheSameStudent) throw new IsuException($"Student already in {GroupInfo.Name}");
            
            _students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            foreach (Student tstudent in students)
            {
                if (tstudent.id != student.id) continue;
                students.Remove(tstudent);
                break;
            }
        }
        public class GroupBuilder
        {
            private  int _maxStudents;
            private GroupID _groupID;
            private List<Student> _students;
            public GroupBuilder WithName(string name)
            {
                _groupID = GroupID.ParseName(name);
                return this;
            
            }
            public GroupBuilder WithMaxAmountOfStudents(int max)
            {
                _maxStudents =max;
                return this;
            }
            public GroupBuilder WithStudents(params Student[] students)
            {
                _students = students.ToList();
                return this;
            }
            public Group Build()
            {
                return new Group(_groupID,_maxStudents,_students ?? new List<Student>());
            }
            public static implicit operator Group(GroupBuilder builder)
            {
                return builder.Build();
            }
        }

       
    }
}