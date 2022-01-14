using System;
using System.Collections.Generic;

namespace IsuExtra
{
    public class Thread
    {
#pragma warning disable SA1401
        public readonly int Num;
#pragma warning restore SA1401
        private int _maxAmountStudents;
        public Thread(int num)
        {
            this.Num = num;
            Students = new List<Student>();
            Lessons = new List<Lesson>();
        }

        public List<Student> Students { get; set; }

        public List<Lesson> Lessons { get; private set; }
        public int AmountOfStudents { get; private set; }

        public int MaxStudents
        {
            get => _maxAmountStudents;
        }

        public void AddStudent(Student student)
        {
            if (student != null)
            {
                if (AmountOfStudents + 1 <= _maxAmountStudents)
                {
                    Students.Add(student);
                    AmountOfStudents++;
                }
            }
        }

        public void ReduceAmountOfStudents(int num)
        {
            AmountOfStudents -= num;
        }

        public void RescaleSizeOfGroup(int num)
        {
            _maxAmountStudents = num;
        }

        public void AddClass(Lesson lesson)
        {
            // check if intersect
            foreach (var lesson1 in Lessons)
            {
                if (lesson1.IsIntersect(lesson))
                {
                    throw new Exception($" cannot add class {lesson}: conflict in schedule {lesson1} and {lesson}");
                }
            }

            // add
            Lessons.Add(lesson);
        }
    }
}