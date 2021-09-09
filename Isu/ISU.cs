using System;
using System.Collections.Generic;
using Isu.Services;
using Isu.Tools;

namespace Isu
{
    public class ISU : IIsuService
    {
        private GroupManager manager;

        public ISU()
        {
            manager = new GroupManager();
        }
        
        public void AddGroup(string name)
        {
            GroupID id = StringProccessor.ParseName(name);
            if (!manager.checkIfGroupExists(id))
            {
                manager.CreateGroup(id);
            }
            else throw new IsuException($"Group{name} already exists");
        }

        public void AddStudent(Group group, string name)
        {
            Student student = new Student(name, group);
            manager.RegisterAStudent(group, student);
        }

        public Student GetStudent(int id)
        {
            var student = manager.FindById(id);
            if (student == null) throw new Exception($"Student with ID{id} is not found");
            return student;
        }

        public Student FindStudent(string name)
        {
            var student = manager.FindByName(name);
            if (student == null) throw new Exception($"Student with ID{name} is not found");
            return student;
        }

        public List<Student> FindStudents(string groupName)
        {
            var students = manager.GetStudentsOfgroup(groupName);
            return students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = manager.GetStudentsOfCourse(courseNumber);
            return students;
        }

        public Group FindGroup(string groupName)
        {
            var group = manager.GetGroup(groupName);
            return group;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var groups = manager.GetGroups(courseNumber);
            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            manager.DeregisterAStudent(student);
            manager.RegisterAStudent(newGroup,student);
        }
    }
}