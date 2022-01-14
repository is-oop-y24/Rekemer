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

        public GroupManager(int amountOfGroupes = 10)
        {
            _dataOfGroupes = new List<Group>[amountOfGroupes];
            for (int i = 0; i < _dataOfGroupes.Length; i++)
            {
                _dataOfGroupes[i] = new List<Group>();
            }
        }

        public List<Group>[] DataOfGroupes
        {
            get => _dataOfGroupes;
            private set => _dataOfGroupes = value;
        }

        public Group AddGroup(Group group)
        {
            GroupID id = group.GroupInfo;

            if (!IsGroupExists(id, this))
            {
                _dataOfGroupes[(int)id.CourseNum].Add(group);
            }
            else
            {
                throw new IsuException($"Group{group.GroupInfo.Name} already exists");
            }

            return group;
        }

        public void AddStudent(Group group, Student student)
        {
            var existingGroup = HasGroup(group.GroupInfo, this);
            if (existingGroup == null)
            {
                throw new IsuException(
                    $"group{group.GroupInfo.Name} doesn't exist");
            }

            group.AddStudent(student);
        }

        public Student GetStudent(int id)
        {
            var student = FindById(id, this);
            if (student == null) throw new Exception($"Student with ID{id} is not found");
            return student;
        }

        public Student FindStudent(string name)
        {
            var student = FindByName(name, this);
            if (student == null) throw new Exception($"Student with ID{name} is not found");
            return student;
        }

        public List<Student> FindStudents(string groupName)
        {
            var students = GetStudentsOfgroup(groupName);
            return students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = GetStudentsOfCourse(courseNumber);
            return students;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            GroupID groupid = student.StudentsGroup;
            Group group = FindGroup(groupid);
            if (group == null) throw new IsuException("There is no group in which this student exists");
            group.DeleteStudent(student);
            newGroup.AddStudent(student);
        }

        public Group FindGroup(GroupID id)
        {
            var group = DataOfGroupes[(int)id.CourseNum].FirstOrDefault(t => t.GroupInfo.Num == id.Num);
            if (group == null) throw new IsuException($"There is no group {id.Name}");
            return group;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = DataOfGroupes[(int)courseNumber];
            if (groups == null) throw new IsuException($"There are no groups in course{courseNumber}");
            return groups;
        }

        private bool IsGroupExists(GroupID id, GroupManager manager)
        {
            var group = HasGroup(id, manager);
            if (group == null) return false;
            return true;
        }

        private Group HasGroup(GroupID id, GroupManager manager)
        {
            CourseNumber courseNum = id.CourseNum;
            var numberOfGroup = id.Num;

            if (manager.DataOfGroupes[(int)courseNum].Count == 0) return null;
            var group = manager.DataOfGroupes[(int)courseNum].FirstOrDefault(t => t.GroupInfo.Num == numberOfGroup);
            return group;
        }

        private Student FindById(int id, GroupManager manager)
        {
            Student student = null;
            for (int i = 0; i < manager.DataOfGroupes.Length; i++)
            {
                if (manager.DataOfGroupes[i] == null) continue;
                for (int j = 0; j < manager.DataOfGroupes[i].Count; j++)
                {
                    var groups = manager.DataOfGroupes[i];
                    for (int k = 0; k < groups.Count; k++)
                    {
                        student = groups[k].Students
                            .FirstOrDefault(t => t.Id == id);
                    }
                }
            }

            return student;
        }

        private Student FindByName(string name, GroupManager manager)
        {
            Student student = null;
            for (int i = 0; i < manager.DataOfGroupes.Length; i++)
            {
                if (manager.DataOfGroupes[i] == null) continue;
                for (int j = 0; j < manager.DataOfGroupes[i].Count; j++)
                {
                    var groups = manager.DataOfGroupes[i];
                    for (int k = 0; k < groups.Count; k++)
                    {
                        student = groups[k].Students
                            .FirstOrDefault(t => t.Name == name);
                    }
                }
            }

            return student;
        }

        private List<Student> GetStudentsOfgroup(string nameOfGroup)
        {
            GroupID groupId = GroupID.ParseName(nameOfGroup);
            Group group = DataOfGroupes[(int)groupId.CourseNum].FirstOrDefault(t => t.GroupInfo.Name == nameOfGroup);
            if (group == null) throw new IsuException($"{nameOfGroup} is empty");
            return group.Students;
        }

        private List<Student> GetStudentsOfCourse(CourseNumber courseNumber)
        {
            List<Student> students = new List<Student>();
            var currCourse = DataOfGroupes[(int)courseNumber];
            foreach (Group tGroup in currCourse)
            {
                students.AddRange(tGroup.Students);
            }

            if (students.Count == 0) throw new IsuException($"There are no students on this {courseNumber} course");
            return students;
        }
    }
}