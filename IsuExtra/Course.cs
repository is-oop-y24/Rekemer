using System;
using System.Collections.Generic;
using System.Linq;

namespace IsuExtra
{
    public enum MegaFaculty
    {
        CompTech,
        TranslInf,
        BioTech,
        SciLife,
        None
    }

    public class Course
    {
        public readonly MegaFaculty Faculty;
        private List<Thread> _threads = new List<Thread>();
        private int maxAmountStudents;

        public Course(MegaFaculty faculty, int amountOfThreads, int maxAmountStudents)
        {
            Faculty = faculty;
            for (int i = 0; i < amountOfThreads; i++)
            {
                var thread = new Thread(i + 1);
                _threads.Add(thread);
            }

            this.maxAmountStudents = maxAmountStudents;
        }

        public Thread GetThread(int num)
        {
            foreach (var thread in _threads)
            {
                if (thread.Num == num)
                {
                    return thread;
                }
            }

            return null;
        }

        public List<Thread> GetThreads()
        {
            return new List<Thread>(_threads);
        }

        public void AddClassToThread(Lesson lesson, int threadNum)
        {
            Thread thread = GetThread(threadNum);
            if (thread == null) throw new Exception($"AddClassToThread There is no thread {threadNum}");
            thread.AddClass(lesson);
        }

        public bool CheckSpaceIfStudentIsAdded(int num)
        {
            int amountOfStudents = 0;
            Thread threadToGet = new Thread(0);
            foreach (var thread in _threads)
            {
                if (thread.Num != num)
                {
                    threadToGet = thread;
                }

                amountOfStudents += thread.AmountOfStudents;
            }

            if (amountOfStudents + 1 > maxAmountStudents)
            {
                throw new Exception($"Faculty{Faculty} is full");
            }

            return true;
        }

        public void UpdateThreadOfStudents(Thread thread)
        {
            var realObject = _threads.FirstOrDefault(t => t.Num == thread.Num);
            if (realObject != null)
            {
                realObject.Students = realObject.Students.Union(thread.Students).ToList();
            }
        }

        public void RemoveStudent(Student student)
        {
            Thread thread = null;
            if (student.Course1.Faculty == Faculty)
            {
                thread = student.Thread1;
            }
            else if (student.Course2.Faculty == Faculty)
            {
                thread = student.Thread2;
            }

            if (thread != null) RemoveFrom(thread, student);
        }

        void RemoveFrom(Thread thread, Student student)
        {
            var thread0 = GetThread(thread.Num);
            thread0.Students.Remove(student);
        }
    }
}