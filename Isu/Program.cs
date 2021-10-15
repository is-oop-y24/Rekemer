using System;
using System.Collections.Generic;
using System.Linq;

namespace Isu
{
    internal class Program
    {
        private static void Main()
        {
            int[] a = new int[10];
            for (int i = 0; i < 2; i++)
            {
                a[i] = i;
            }

            //List<int> b = a;
            Console.WriteLine();

            // GroupManager manager = new GroupManager();
            // manager.AddGroup("M3209");
            // manager.AddGroup("M3212");
            // manager.AddGroup("M3201");
            // manager.AddGroup("M3301");
            // manager.AddGroup("M3304");
            // manager.AddGroup("M3327");
            // manager.AddGroup("M3316");
            // Group group0 = Builder.GroupBuilder.WithName("M3209");
            // Group group1 = Builder.GroupBuilder.WithName("M3212");
            // Group group2 = Builder.GroupBuilder.WithName("M3201");
            // Group group3 = Builder.GroupBuilder.WithName("M3301");
            // Group group4 = Builder.GroupBuilder.WithName("M3304");
            // Group group5 = Builder.GroupBuilder.WithName("M3316");
            //
            // manager.AddStudent(group0, "ilia");
            // manager.AddStudent(group5, "kiriil");
            // manager.AddStudent(group5, "Maria");
            // manager.AddStudent(group4, "Oleg");
            // CourseNumber a = 2;
            // var students = manager.FindStudents(a);
            // var group = manager.FindGroups(2);
            // Student student1 = Builder.StudentBuilder.WithName("ilia").WithGroup(group0.GroupInfo);
            // Student student2 = Builder.StudentBuilder.WithName("kiriil").WithGroup(group5.GroupInfo);
            // manager.ChangeStudentGroup(student1, group5);
            // manager.ChangeStudentGroup(student2, group0);
        }
    }
}