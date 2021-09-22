using System;

namespace Isu
{
    internal class Program
    {
        private static void Main()
        {
            GroupManager manager = new GroupManager();
            manager.AddGroup("M3209");
            manager.AddGroup("M3212");
            manager.AddGroup("M3201");
            manager.AddGroup("M3301");
            manager.AddGroup("M3304");
            manager.AddGroup("M3327");
            manager.AddGroup("M3316");
            Group group0 = new Group("M3209");
            Group group0_1 = new Group("M3212");
            Group group0_2 = new Group("M3201");
            Group group1 = new Group("M3301");
            Group group2 = new Group("M3304");
            Group group3 = new Group("M3316");

            manager.AddStudent(group0, "ilia");
            manager.AddStudent(group0_1, "kiriil");
            manager.AddStudent(group0_1, "Maria");
            manager.AddStudent(group0_2, "Oleg");
            CourseNumber a = 2;
            var students = manager.FindStudents(a);
            var group = manager.FindGroups(2);
            Student student_0 = new Student("ilia", group0);
            Student student_1 = new Student("kiriil", group0_1);
            manager.ChangeStudentGroup(student_0, group0_1);
            manager.ChangeStudentGroup(student_1, group0);
        }
    }
}