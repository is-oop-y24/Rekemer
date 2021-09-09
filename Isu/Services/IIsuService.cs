using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Isu.Services
{
    public interface IIsuService
    {
        void AddGroup(string name);
        void AddStudent(Group group, string name);

        Student GetStudent(int id);
        Student FindStudent(string name);
        List<Student> FindStudents(string groupName);
        List<Student> FindStudents(CourseNumber courseNumber);

        Group FindGroup(string groupName);
        List<Group> FindGroups(CourseNumber courseNumber);

        void ChangeStudentGroup(Student student, Group newGroup);
    }
}