using System.Linq;
using Isu.Services;
using NUnit.Framework;

namespace Isu.Tests
{
    public class TestsIsu
    {
        private IIsuService _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new GroupManager();
        }
        
        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            
            Group group = Builder.GroupBuilder.WithName("M3209").WithMaxAmountOfStudents(10);
            
            _manager.AddGroup(group);
            Student student = Builder.StudentBuilder.WithName("ilia").WithGroup(group.GroupInfo);
            _manager.AddStudent(group, student);
            string nameOfStudent = _manager.FindGroup("M3209").students[0].name;
            Assert.AreEqual("ilia",nameOfStudent );
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            
            Group group = Builder.GroupBuilder.WithName("M3209").WithMaxAmountOfStudents(1);
            _manager.AddGroup(group);
            Student student = Builder.StudentBuilder.WithName("ilia").WithGroup(group.GroupInfo);
            for (int i = 0; i < group.maxStudents; i++)
            {
                _manager.AddStudent(group,student);
            }
            _manager.AddStudent(group,student);
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Group group = Builder.GroupBuilder.WithName("M1232").WithMaxAmountOfStudents(21);
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            
         
            Group group0 =Builder.GroupBuilder.WithName("M3209").WithMaxAmountOfStudents(21);
            Group group1 = Builder.GroupBuilder.WithName("M3212").WithMaxAmountOfStudents(21);
            _manager.AddGroup(group0);
            _manager.AddGroup(group1);
            Student student0 = Builder.StudentBuilder.WithName("ilia").WithGroup(group0.GroupInfo);
            Student student1 = Builder.StudentBuilder.WithName("kiriil").WithGroup(group1.GroupInfo);
            Student student2 = Builder.StudentBuilder.WithName("Maria").WithGroup(group1.GroupInfo);
            _manager.AddStudent(group0, student0);
            _manager.AddStudent(group0, student2);
            _manager.AddStudent(group1, student1);
            
            _manager.ChangeStudentGroup(student0, group1);
            _manager.ChangeStudentGroup(student1, group0);
            var StudentsIn_09 = _manager.FindStudents("M3209");
            var StudentsIn_12 = _manager.FindStudents("M3212");
            bool isTransferred_fromM309 = _manager.FindGroup("M3212").students.Any(t => t.name == "ilia");
            bool isTransferred_fromM312 = _manager.FindGroup("M3209").students.Any(t => t.name == "kiriil");
            Assert.AreEqual(isTransferred_fromM309, isTransferred_fromM312);
        }

        [Test]
        public void SameGroupesAreAdded_ThrowException()
        {
            
            Group group0 =Builder.GroupBuilder.WithName("M3209").WithMaxAmountOfStudents(21);
            _manager.AddGroup(group0);
            _manager.AddGroup(group0);
        }
    }

    public class Group_Tests
    {
        [Test]
        public void GroupIsNotNamedCorrectly_ThrowException()
        {
            Group groupToFail = Builder.GroupBuilder.WithName("M23b2").WithMaxAmountOfStudents(21);
        }

        [Test]
        public void CanGroupIdNumberBeFormed_IdNumberIsFormed()
        {
            Group group = Builder.GroupBuilder.WithName("M3398");
            int courseNum = group.GroupInfo.courseNum;
            Assert.AreEqual(398, group.GroupInfo.num + courseNum * 100);
        }
    }
}