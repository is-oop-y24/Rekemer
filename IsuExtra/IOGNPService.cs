using System.Collections.Generic;

namespace IsuExtra
{
    public interface IOGNPService
    {
        void AddCourse(string megaFaculty, int amountOfThreads,params  int[] sizeOfGroups);
        void AddStudentToCourse(string megaFaculty, int threadNum, Student student);
        void RemoveStudentFromCourse(string faculty, Student student);
        List<Thread> GetThreads(string faculty);
        List<Student> GetStudentOfThread(string faculty, int threadNum);

        List<Student> GetNonSubscribedStudents();
    }
}