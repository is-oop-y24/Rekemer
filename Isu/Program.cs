using System;

namespace Isu
{
    internal class Program
    {
        private static void Main()
        {
            ISU isu = new ISU();
            isu.AddGroup("M3209");
            isu.AddGroup("M3212");
            isu.AddGroup("M3201");
            isu.AddGroup("M3301");
            isu.AddGroup("M3304");
            isu.AddGroup("M3327");
            isu.AddGroup("M3316");
            Group group0 = new Group("M3209");
            Group group0_1 = new Group("M3212");
            Group group0_2 = new Group("M3201");
            Group group1= new Group("M3301");
            Group group2 = new Group("M3304");
            Group group3= new Group("M3316");
         //  Student student = new Student("Ilia", group);
            isu.AddStudent(group0, "ilia");
            isu.AddStudent(group0_1, "kiriil");
            isu.AddStudent(group0_1, "Maria");
            isu.AddStudent(group0_2, "Oleg");
            CourseNumber a = 2;
            var students = isu.FindStudents(a);
            var group = isu.FindGroups(2);
            Student student_0 = new Student("ilia", group0);
            Student student_1 = new Student("kiriil", group0_1);
            isu.ChangeStudentGroup(student_0,group0_1);
            isu.ChangeStudentGroup(student_1,group0);
            // int id = StringProccessor.GetID("ilia");
            // student = isu.GetStudent(id);
            // student = isu.FindStudent("ilia");


        }
    }
}
