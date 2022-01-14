using System;
using System.Linq;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class TestsIsu
    {
        private IIsuService _manager;
        private Group.GroupBuilder groupBuilder;
        private Student.StudentBuilder studentBuilder;
        [SetUp]
        public void Setup()
        {
            _manager = new GroupManager();
            groupBuilder = new Group.GroupBuilder();
            studentBuilder = new Student.StudentBuilder();

        }
        
        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
          
            

            Group group = groupBuilder.WithName("M3209").WithMaxAmountOfStudents(10);
            _manager.AddGroup(group);
            Student student = studentBuilder.WithName("ilia").WithGroup(group.GroupInfo);
            _manager.AddStudent(group, student);
            string nameOfStudent = _manager.FindGroup("M3209").Students[0].Name;
            Assert.AreEqual("ilia", nameOfStudent );
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            try
            {
                Group group = groupBuilder.WithName("M3209").WithMaxAmountOfStudents(1);
                _manager.AddGroup(group);
                Student student = studentBuilder.WithName("ilia").WithGroup(group.GroupInfo);
                for (int i = 0; i < group.MaxStudents; i++)
                {
                    _manager.AddStudent(group, student);
                }
                _manager.AddStudent(group, student);
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
            Assert.Fail();

            
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            try
            {
                groupBuilder.WithName("M3632").WithMaxAmountOfStudents(21);
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
       

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group0 = groupBuilder.WithName("M3209").WithMaxAmountOfStudents(21);
            Group group1 = groupBuilder.WithName("M3212").WithMaxAmountOfStudents(21);
            _manager.AddGroup(group0);
            _manager.AddGroup(group1);
            Student student0 = studentBuilder.WithName("ilia").WithGroup(group0.GroupInfo);
            Student student1 = studentBuilder.WithName("kiriil").WithGroup(group1.GroupInfo);
            Student student2 = studentBuilder.WithName("Maria").WithGroup(group1.GroupInfo);
            _manager.AddStudent(group0, student0);
            _manager.AddStudent(group0, student2);
            _manager.AddStudent(group1, student1);
            _manager.ChangeStudentGroup(student0, group1);
            _manager.ChangeStudentGroup(student1, group0);
            bool isTransferred_fromM309 = _manager.FindGroup("M3212").Students.Any(t => t.Name == "ilia");
            bool isTransferred_fromM312 = _manager.FindGroup("M3209").Students.Any(t => t.Name == "kiriil");
            Assert.AreEqual(isTransferred_fromM309, isTransferred_fromM312);
        }

        [Test]
        public void SameGroupesAreAdded_ThrowException()
        {
            try
            {
                Group group0 = groupBuilder.WithName("M3209").WithMaxAmountOfStudents(21);
                _manager.AddGroup(group0);
                _manager.AddGroup(group0);
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
            Assert.Fail();
          
           
        }
    }

    public class Group_Tests
    {
      
        private Group.GroupBuilder groupBuilder;
        
        [SetUp]
        public void Setup()
        {
            groupBuilder = new Group.GroupBuilder();
        }
        
        [Test]
        public void GroupIsNotNamedCorrectly_ThrowException()
        {
            try
            {
                Group groupToFail = groupBuilder.WithName("M23b2").WithMaxAmountOfStudents(21);
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
            Assert.Fail();
          
        }

        [Test]
        public void CanGroupIdNumberBeFormed_IdNumberIsFormed()
        {
            Group group = groupBuilder.WithName("M3398");
            int courseNum = (int)group.GroupInfo.CourseNum;
            Assert.AreEqual(398, group.GroupInfo.Num + courseNum * 100);
        }
    }
}