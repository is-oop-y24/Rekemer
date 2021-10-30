using System;
using System.Collections.Generic;

namespace IsuExtra
{
    public class Thread
    {
        public readonly int Num;
        public List<Student> Students { get; set; }
        public List<Lesson> Lessons { get; private set; }
        public int AmountOfStudents { get; private set; }


        public Thread(int num)
        {
            this.Num = num;
            Students = new List<Student>();
            Lessons = new List<Lesson>();
        }

        public void AddStudent(Student student)
        {
            if (student != null)
            {
                Students.Add(student);
                AmountOfStudents++;
            }
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