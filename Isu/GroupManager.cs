using System;
using System.Collections.Generic;

using Isu.Services;
using Isu.Tools;

namespace Isu
{
    public class GroupManager : IIsuService
    {
        private List<Group>[] _dataOfGroupes;

        public List<Group>[] dataOfGroupes
        {
            get => _dataOfGroupes;
            private set => _dataOfGroupes = value;
        }

        private GroupSearcher _groupSearcher;
        private StudentSearcher _studentSearcher;
        private Register _register;
        private Checker _checker;


        public GroupManager(int amountOfGroupes = 10)
        {
            _groupSearcher = new GroupSearcher();
            _studentSearcher = new StudentSearcher();
            _register = new Register();
            _checker = new Checker();

            _dataOfGroupes = new List<Group>[amountOfGroupes];
            for (int i = 0; i < _dataOfGroupes.Length; i++)
            {
                _dataOfGroupes[i] = new List<Group>();
            }
        }


        public void AddGroup(string name)
        {
            GroupID id = StringProccessor.ParseName(name);
            if (!(_checker.CheckIfGroupExists(id, this)))
            {
                _dataOfGroupes[id.courseNum].Add(new Group(id.courseNum, id.num));
            }
            else throw new IsuException($"Group{name} already exists");
        }

        public void AddStudent(Group group, string name)
        {
            Student student = new Student(name, group);
            var existingGroup = _checker.HasGroup(group.GroupInfo, this);
            if (existingGroup == null)
                throw new IsuException
                    ($"group{StringProccessor.FormAName(group.GroupInfo)} doesn't exist");
            _register.RegisterAStudent(group, student, this, _checker);
        }

        public Student GetStudent(int id)
        {
            var student = _studentSearcher.FindById(id, this);
            if (student == null) throw new Exception($"Student with ID{id} is not found");
            return student;
        }

        public Student FindStudent(string name)
        {
            var student = _studentSearcher.FindByName(name, this);
            if (student == null) throw new Exception($"Student with ID{name} is not found");
            return student;
        }

        public List<Student> FindStudents(string groupName)
        {
            var students = _studentSearcher.GetStudentsOfgroup(groupName, this);
            return students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = _studentSearcher.GetStudentsOfCourse(courseNumber, this);
            return students;
        }

        public Group FindGroup(string groupName)
        {
            var group = _groupSearcher.GetGroup(groupName, this);
            return group;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = _groupSearcher.GetGroups(courseNumber, this);
            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            _register.DeregisterAStudent(student, _groupSearcher, this);
            _register.RegisterAStudent(newGroup, student, this, _checker);
        }
    }
}