using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Isu.Services
{
    public interface IIsuService
    {
        Group AddGroup(Group group);
        void AddStudent(Group group, Student student);

        Student GetStudent(int id);
        Student FindStudent(string name);
        List<Student> FindStudents(string groupName);
        List<Student> FindStudents(CourseNumber courseNumber);

        Group FindGroup(GroupID groupName);
        List<Group> FindGroups(CourseNumber courseNumber);

        void ChangeStudentGroup(Student student, Group newGroup);
    }
}