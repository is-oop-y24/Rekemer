using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private readonly int _maxStudents;
        private string _name;

        private List<Student> _students = new List<Student>();
        public List<Student> students { get => _students; }
        public GroupID GroupInfo { get; private set; }

        public Group(string name, int maxStudents = 20)
        {
            Name = name;
            _maxStudents = maxStudents;
        }

        public void AddingAStudent(Student student)
        {
            if (_students.Count == _maxStudents)
                throw new IsuException($"Student can not be added in the group {_name}, the group is full");
            string name = student.name;
            bool isThereAlreadyTheSameStudent = _students.Any(t => t.name == name);
            if (isThereAlreadyTheSameStudent) throw new IsuException($"Student already in {_name}");
            _students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            foreach (Student tstudent in students)
            {
                if (tstudent.id == student.id)
                {
                    students.Remove(tstudent);
                    break;
                }
            }
        }

        public Group(CourseNumber Course, int num, int maxStudents = 20)
        {
            GroupInfo = new GroupID(Course, num);
            _name = StringProccessor.FormAName(GroupInfo);
            _maxStudents = maxStudents;
        }


        public string Name
        {
            get => _name;
            private set
            {
                GroupInfo = StringProccessor.ParseName(value);
                _name = value;
            }
        }
    }

    public struct GroupID
    {
        public readonly CourseNumber courseNum;
        public readonly int num;

        public GroupID(CourseNumber courseNum, int num)
        {
            this.courseNum = courseNum;
            this.num = num;
        }
    }
}