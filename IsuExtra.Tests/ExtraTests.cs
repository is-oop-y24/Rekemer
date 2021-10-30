using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using IsuExtra;

namespace IsuExtra.Tests
{
    public class ExtraTests
    {
        private GroupManager _manager;
        private Group.GroupBuilder groupBuilder;
        private Student.StudentBuilder studentBuilder;
        private Student student;
        private Group IliasGroup;

        [SetUp]
        public void Setup()
        {
            _manager = new GroupManager();
            groupBuilder = new Group.GroupBuilder();
            studentBuilder = new Student.StudentBuilder();
            student = studentBuilder.WithGroup("C3209").WithName("Ilia");
            IliasGroup = groupBuilder.WithName("C3209").WithMaxAmountOfStudents(4);
            _manager.AddGroup(IliasGroup);
            _manager.AddStudent(IliasGroup, student);
        }

        [Test]
        public void AddCourse_CourseIsAdded()
        {
            _manager.AddCourse(MegaFaculty.BioTech, 2, 14);
            Assert.AreEqual(MegaFaculty.BioTech, _manager.FindCourse(MegaFaculty.BioTech).Faculty);
        }


        [Test]
        public void AddTooManyStudentsToCourse_ThrowException()
        {
            _manager.AddCourse(MegaFaculty.BioTech, 2, 1);
            _manager.AddStudentToCourse(MegaFaculty.BioTech, 1, student);
            Assert.Throws<Exception>(delegate
            {
                TryToAddTooManyStudents();
                ;
            });
        }

        private void TryToAddTooManyStudents()
        {
            var anotherStudent = studentBuilder.WithGroup("C3209").WithName("Someone");
            _manager.AddStudentToCourse(MegaFaculty.BioTech, 1, anotherStudent);
        }

        [Test]
        public void DeleteStudentFromCourse_StudentIsDeleted()
        {
            _manager.AddCourse(MegaFaculty.BioTech, 2, 1);
            _manager.AddStudentToCourse(MegaFaculty.BioTech, 1, student);
            _manager.RemoveStudentFromCourse(MegaFaculty.BioTech, student);
            Assert.AreEqual(null, student.Course1);
        }

        [Test]
        public void RegisterInSameMegaFaculty_ThrowException()
        {
            Assert.Throws<Exception>(delegate { TryToAddToSameMegaFaculty(); });
        }

        public void TryToAddToSameMegaFaculty()
        {
            _manager.AddCourse(MegaFaculty.CompTech, 2, 1);
            _manager.AddStudentToCourse(MegaFaculty.CompTech, 1, student);
        }
    }
}