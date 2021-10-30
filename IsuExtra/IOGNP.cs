using System.Collections.Generic;

namespace IsuExtra
{
    public interface IOGNP
    {
        public void AddCourse(MegaFaculty megaFaculty, int amountOfThreads, int maxStudentsAmount);
        public void AddStudentToCourse(MegaFaculty megaFaculty, int threadNum, Student student);
        public void RemoveStudentFromCourse(MegaFaculty faculty, Student student);
        public List<Thread> GetThreads(MegaFaculty faculty);
        public List<Student> GetStudentOfThread(MegaFaculty faculty, int threadNum);

        public List<Student> GetNonSubscribedStudents();
    }
}