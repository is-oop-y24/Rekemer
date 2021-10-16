using System;
using System.Collections.Generic;
using System.Linq;
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

       
        private StudentSearcher _studentSearcher;

        private bool IsGroupExists(GroupID id, GroupManager manager)
        {
            var group = HasGroup(id, manager);
            if (group == null) return false;
            return true;
        }

        private Group HasGroup(GroupID id, GroupManager manager)
        {
            CourseNumber courseNum = id.courseNum;
            var numberOfGroup = id.num;

            if (manager.dataOfGroupes[(int)courseNum].Count == 0) return null;
            var group = manager.dataOfGroupes[(int)courseNum].FirstOrDefault(t => t.GroupInfo.num == numberOfGroup);
            return group;
        }
        public GroupManager(int amountOfGroupes = 10)
        {
            
            _studentSearcher = new StudentSearcher();


            _dataOfGroupes = new List<Group>[amountOfGroupes];
            for (int i = 0; i < _dataOfGroupes.Length; i++)
            {
                _dataOfGroupes[i] = new List<Group>();
            }
        }


        public Group AddGroup(Group group)
        {
            GroupID id = group.GroupInfo;
            
            if (!(IsGroupExists(id, this)))
            {
                _dataOfGroupes[(int)id.courseNum].Add(group);
            }
            else throw new IsuException($"Group{group.GroupInfo.Name} already exists");

            return group;
        }

        public void AddStudent(Group group,Student student)
        {
            var existingGroup = HasGroup(group.GroupInfo, this);
            if (existingGroup == null)
                throw new IsuException
                    ($"group{group.GroupInfo.Name} doesn't exist");
            group.AddStudent(student);
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

     
        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            GroupID groupid = student.studentsGroup;
            Group group = FindGroup(groupid);
            if (group == null) throw new IsuException("There is no group in which this student exists");
            group.DeleteStudent(student);
            newGroup.AddStudent(student);
        }
        public Group FindGroup( GroupID id)
        {
           
            var group = dataOfGroupes[(int)id.courseNum].FirstOrDefault(t => t.GroupInfo.num == id.num);
            if (group == null) throw new IsuException($"There is no group {id.Name}");
            return group;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = dataOfGroupes[(int)courseNumber];
            if (groups == null) throw new IsuException($"There are no groups in course{courseNumber}");
            return groups;
        }
    }
}