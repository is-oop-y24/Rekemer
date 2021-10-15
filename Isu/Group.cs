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
        public GroupID GroupInfo { get; private set; }
        
        private List<Student> _students;

        public List<Student> students => _students;

        private Group(GroupID id, int maxStudents, List<Student> students)
        {
            GroupInfo =  id;
            this.maxStudents = maxStudents;
            _students = students;
        }
        public void AddingAStudent(Student student)
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

    public class GroupID
    {
        public readonly CourseNumber courseNum;
        public readonly int num;
        public string Name { get; private set; }

        private GroupID(CourseNumber courseNum, int num)
        {
            this.courseNum = courseNum;
            this.num = num;
            Name = FormAName(courseNum, num);
        }

        public static implicit operator string(GroupID id)
        {
            return id.Name;
        }
        public static implicit operator GroupID(string name)
        {
            GroupID id = ParseName(name);
            return id;
        }
        public static  GroupID ParseName(string name)
        {
            int number;
            string numSubstring = name.Substring(1);
            bool areInts = int.TryParse(numSubstring, out number);
            if (name.Length != 5 || name[0] != 'M' || name[1] != '3' || !areInts)
                throw new IsuException("Invalid group name (M3XYY)");

            GroupID groupInfo = new GroupID(int.Parse(name[2].ToString()), byte.Parse(numSubstring.Substring(2)));
            return groupInfo;
        }

        public static string FormAName(CourseNumber courseNum, int groupNumber )
        {
            int course = courseNum;
            string name = "M3" + course.ToString();
            if (groupNumber < 10) name += "0" + groupNumber.ToString();
            else name += groupNumber.ToString();
            return name;
        }

        
    }
}