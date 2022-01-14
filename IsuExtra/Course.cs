using System;
using System.Collections.Generic;
using System.Linq;

namespace IsuExtra
{
    public class Course
    {
#pragma warning disable SA1401
        public readonly string Faculty;
#pragma warning restore SA1401
        private List<Thread> _threads = new List<Thread>();

        public Course(string faculty, int amountOfThreads, params int[] sizeOfGroups)
        {
            Faculty = faculty;
            if (amountOfThreads == sizeOfGroups.Length)
            {
                for (int i = 0; i < amountOfThreads; i++)
                {
                    var thread = new Thread(i + 1);
                    thread.RescaleSizeOfGroup(sizeOfGroups[i]);
                    _threads.Add(thread);
                }
            }
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
            Thread threadToGet = new Thread(-1);
            foreach (var thread in _threads)
            {
                if (thread.Num == num)
                {
                    threadToGet = thread;
                    break;
                }
            }

            if (threadToGet.Num == -1) throw new Exception($"There is no group {num} in faculty{Faculty}");

            if (threadToGet.AmountOfStudents + 1 > threadToGet.MaxStudents)
            {
                throw new Exception(
                    $"Group {threadToGet.Num} in Faculty{Faculty} hasn't got any spaces for adding student");
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

        private void RemoveFrom(Thread thread, Student student)
        {
            var thread0 = GetThread(thread.Num);
            thread0.ReduceAmountOfStudents(1);
            thread0.Students.Remove(student);
        }
    }
}