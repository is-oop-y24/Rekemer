using System;
using System.Linq;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests_Isu
    {
        private IIsuService _isuService;

        // [SetUp]
        // public void Setup()
        // {
        //     //TODO: implement
        //     _isuService = null;
        // }
        //
        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            try
            {
                ISU isu = new ISU();
                isu.AddGroup("M3209");
                Group group = new Group("M3209");
                isu.AddStudent(group, "Ilia");
                isu.AddStudent(group, "Ilia");
                Assert.Fail();
            }
            catch (IsuException e)
            {
                
            }
            
        }
        
        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            // Assert.Catch<IsuException>(() =>
            // {
            //     
            // });
            try
            {
                var isu = new ISU();
                isu.AddGroup("M3209");
                Group group = new Group("M3209");
                for (int i = 0; i < 21; i++)
                {
                    isu.AddStudent(group, i.ToString());
                }
                Assert.Fail();
            }
            catch (IsuException e)
            {
                
            }
            
        }
        
        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            try
            {
                Group group = new Group("M1232");
                Assert.Fail();
            }
            catch (IsuException e)
            {
               
            }
        }
        
        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var isu = new ISU();
            Group group0 = new Group("M3209");
            Group group0_1 = new Group("M3212");
            isu.AddGroup("M3209");
            isu.AddGroup("M3212");
            isu.AddStudent(group0, "ilia");
            isu.AddStudent(group0_1, "kiriil");
            Student student_0 = new Student("ilia", group0);
            Student student_1 = new Student("kiriil", group0_1);
            isu.ChangeStudentGroup(student_0,group0_1);
            isu.ChangeStudentGroup(student_1,group0);
            var StudentsIn_09 = isu.FindStudents("M3209");
            var StudentsIn_12 = isu.FindStudents("M3212");
            if(StudentsIn_09.Count != 1)Assert.Fail();
            if(StudentsIn_12.Count != 1)Assert.Fail();
            bool isTransferred_fromM309 = isu.FindGroup("M3212").students.Any(t => t.Name == "ilia");
            bool isTransferred_fromM312= isu.FindGroup("M3209").students.Any(t => t.Name == "kiriil");
            Assert.AreEqual(isTransferred_fromM309,isTransferred_fromM312);
          
           
            
            
            
        }
        [Test]
        public void Can_Add_Group_More_Than_Once()
        {
            try
            {
                var isu = new ISU();
                isu.AddGroup("M3209");
                isu.AddGroup("M3209");
                Assert.Fail();
            }
            catch (IsuException e)
            {
                
            }
           
        }

       
}

public class Group_Tests
{
    [Test]
    public void Group_Is_Not_Named_Correctly()
    {
        try
        {
            Group b = new Group("M23b2");
        }
        catch (IsuException e)
        {
        }
    }

    [Test]
    public void Group_Is_Named_Correctly()
    {
        try
        {
            Group b = new Group("M3398");
            Assert.AreEqual("M3398", b.Name);
        }
        catch (IsuException e)
        {
            Assert.Fail();
        }
    } [Test]
    public void Can_Group_Id_Number_Be_Formed()
    {
        try
        {
            Group b = new Group("M3398");
            int courseNum = b.groupInfo.courseNum;
            Assert.AreEqual(398, b.groupInfo.num +  courseNum* 100);
        }
        catch (IsuException e)
        {
            Assert.Fail();
        }
    }
}
}