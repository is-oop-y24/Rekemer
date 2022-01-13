using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using NUnit.Framework;
using IsuExtra;

namespace IsuExtra.Tests
{
    public class IsuExtraTests
    {
        private OgnpService _manager;
        private Group.GroupBuilder groupBuilder;
        private Student.StudentBuilder studentBuilder;
        private Student student;
        private Group IliasGroup;

        [SetUp]
        public void Setup()
        {
            _manager = new OgnpService();
            groupBuilder = new Group.GroupBuilder();
            studentBuilder = new Student.StudentBuilder();
            List<string> faculties = new List<string> {"BioTech", "TranslInf", "CompTech", "SciLife"};
            List<char> prefixes = new List<char> {'B', 'T', 'C', 'S'};
            MegaFaculty.Instance.AddFaculty(faculties, prefixes);
            student = studentBuilder.WithGroup("C3209").WithName("Ilia");
            IliasGroup = groupBuilder.WithName("C3209").WithMaxAmountOfStudents(4);
            Lesson lesson = new Lesson("someone", DaysOfWeek.Monday,
                new TimeInterval(new Vector2(11, 00), new Vector2(13, 00)), 12);
            IliasGroup.AddLessons(new List<Lesson>() {lesson});
            _manager.AddGroup(IliasGroup);
            _manager.AddStudent(IliasGroup, student);
        }

        [Test]
        public void AddCourse_CourseIsAdded()
        {
            _manager.AddCourse("BioTech", 2, new[] {14, 15});
            Assert.AreEqual("biotech", _manager.FindCourse("BioTech").Faculty);
        }


        [Test]
        public void AddTooManyStudentsToCourse_ThrowException()
        {
            int[] sizesOfgroups = new int[] {1, 2};

            _manager.AddCourse("BioTech", 2, sizesOfgroups);
            _manager.AddStudentToCourse("BioTech", 1, student);
            Assert.Throws<Exception>(delegate
            {
                TryToAddTooManyStudents();
                ;
            });
        }

        private void TryToAddTooManyStudents()
        {
            var anotherStudent = studentBuilder.WithGroup("C3209").WithName("Someone");
            _manager.AddStudentToCourse("BioTech", 1, anotherStudent);
        }

        [Test]
        public void DeleteStudentFromCourse_StudentIsDeletedInStudentClass()
        {
            _manager.AddCourse("BioTech", 2, new[] {1, 3});
            _manager.AddStudentToCourse("BioTech", 1, student);
            _manager.RemoveStudentFromCourse("BioTech", student);
            Assert.AreEqual(null, student.Course1);
        }

        [Test]
        public void DeleteStudentFromCourse_StudentIsDeletedInManagerClass()
        {
            _manager.AddCourse("BioTech", 2, new[] {1, 3});
            _manager.AddStudentToCourse("BioTech", 1, student);
            _manager.RemoveStudentFromCourse("BioTech", student);
            Assert.AreEqual(0, _manager.FindCourse("BioTech").GetThread(1).AmountOfStudents);
        }

        [Test]
        public void RegisterInSameMegaFaculty_ThrowException()
        {
            Assert.Throws<Exception>(delegate { TryToAddToSameMegaFaculty(); });
        }

        public void TryToAddToSameMegaFaculty()
        {
            _manager.AddCourse("CompTech", 2, new[] {1, 3});
            _manager.AddStudentToCourse("CompTech", 1, student);
        }

        [Test]
        public void AddStudentWithIntersectingSchedule_StudentIsNotAdded()
        {
            _manager.AddCourse("SciLife", 2, new[] {1, 3});
            Lesson lesson = new Lesson("someone", DaysOfWeek.Monday,
                new TimeInterval(new Vector2(12, 00), new Vector2(14, 00)), 22);
            _manager.AddClassToCourse(lesson, "SciLife", 1);
            _manager.AddStudentToCourse("SciLife", 1, student);
            Assert.AreEqual(false, _manager.GetStudentOfThread("SciLife", 1).Any(t => t.Name == student.Name));
        }
    }
}