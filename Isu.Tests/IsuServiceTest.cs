using System.Linq;
using NUnit.Framework;

namespace Isu.Tests
{
    public class TestsIsu
    {
        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            GroupManager manager = new GroupManager();
            manager.AddGroup("M3209");
            Group group = new Group("M3209", 10);
            manager.AddStudent(group, "Ilia");

            Assert.AreEqual("Ilia", manager.FindGroup("M3209").students[0].name);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            GroupManager manager = new GroupManager();
            manager.AddGroup("M3209");
            Group group = new Group("M3209", 10);
            for (int i = 0; i < 21; i++)
            {
                manager.AddStudent(group, i.ToString());
            }
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Group group = new Group("M1232", 21);
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            GroupManager manager = new GroupManager();
            Group group0 = new Group("M3209", 21);
            Group group0_1 = new Group("M3212", 21);

            manager.AddGroup("M3209");
            manager.AddGroup("M3212");
            manager.AddStudent(group0, "ilia");
            manager.AddStudent(group0_1, "kiriil");
            Student student_0 = new Student("ilia", group0);
            Student student_1 = new Student("kiriil", group0_1);
            manager.ChangeStudentGroup(student_0, group0_1);
            manager.ChangeStudentGroup(student_1, group0);
            var StudentsIn_09 = manager.FindStudents("M3209");
            var StudentsIn_12 = manager.FindStudents("M3212");
            if (StudentsIn_09.Count != 1) Assert.Fail();
            if (StudentsIn_12.Count != 1) Assert.Fail();
            bool isTransferred_fromM309 = manager.FindGroup("M3212").students.Any(t => t.name == "ilia");
            bool isTransferred_fromM312 = manager.FindGroup("M3209").students.Any(t => t.name == "kiriil");
            Assert.AreEqual(isTransferred_fromM309, isTransferred_fromM312);
        }

        [Test]
        public void SameGroupesAreAdded_ThrowException()
        {
            GroupManager manager = new GroupManager();
            manager.AddGroup("M3209");
            manager.AddGroup("M3209");
        }
    }

    public class Group_Tests
    {
        [Test]
        public void GroupIsNotNamedCorrectly_ThrowException()
        {
            Group b = new Group("M23b2", 21);
        }

        [Test]
        public void CanGroupIdNumberBeFormed_IdNumberIsFormed()
        {
            Group b = new Group("M3398");
            int courseNum = b.GroupInfo.courseNum;
            Assert.AreEqual(398, b.GroupInfo.num + courseNum * 100);
        }
    }
}