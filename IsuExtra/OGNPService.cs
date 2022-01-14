using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace IsuExtra
{
    public class OgnpService : IOGNPService
    {
        private List<Group>[] _dataOfGroupes;
        public OgnpService(int amountOfGroupes = 10)
        {
            _dataOfGroupes = new List<Group>[amountOfGroupes];
            for (int i = 0; i < _dataOfGroupes.Length; i++)
            {
                _dataOfGroupes[i] = new List<Group>();
            }

            Courses = new List<Course>();
        }

        public List<Course> Courses { get; private set; }

        public List<Group>[] DataOfGroupes
        {
            get => _dataOfGroupes;
            private set => _dataOfGroupes = value;
        }

        public Group AddGroup(Group group)
        {
            GroupID id = group.GroupInfo;

            if (!IsGroupExists(id, this))
            {
                _dataOfGroupes[(int)id.CourseNum].Add(group);
            }
            else
            {
                throw new IsuException($"Group{group.GroupInfo.Name} already exists");
            }

            return group;
        }

        public void AddStudent(Group group, Student student)
        {
            var existingGroup = HasGroup(group.GroupInfo, this);
            if (existingGroup == null)
            {
                throw new IsuException(
                    $"group{group.GroupInfo.Name} doesn't exist");
            }

            group.AddStudent(student);
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var students = GetStudentsOfCourse(courseNumber);
            return students;
        }

        public void AddClassToCourse(Lesson lesson, string megaFaculty, int numThread)
        {
            var course = FindCourse(megaFaculty);
            course.AddClassToThread(lesson, numThread);
        }

        public Group FindGroup(GroupID id)
        {
            var group = DataOfGroupes[(int)id.CourseNum].FirstOrDefault(t => t.GroupInfo.Num == id.Num);
            if (group == null) throw new IsuException($"There is no group {id.Name}");
            return group;
        }

        public void AddCourse(string megaFaculty, int amountOfThreads, params int[] sizeOfGroups)
        {
            var amountOfFaculties = MegaFaculty.Instance.Faculties.Count;
            megaFaculty = megaFaculty.ToLower();
            if (MegaFaculty.Instance.IsFacultyExists(megaFaculty) && amountOfFaculties > Courses.Count)
            {
                foreach (var course1 in Courses)
                {
                    if (course1.Faculty == megaFaculty)
                    {
                        throw new Exception($"{megaFaculty} has already been added");
                    }
                }

                Course course = new Course(megaFaculty, amountOfThreads, sizeOfGroups);
                Courses.Add(course);
            }
        }

        public void AddStudentToCourse(string faculty, int threadNum, Student student)
        {
            // check if student has other megafaculty
            if (student.GroupId != null)
            {
                var course = FindCourse(faculty);
                if (course != null)
                {
                    var megaFaculty = student.GroupId.Faculty;
                    if (MegaFaculty.Instance.IsFacultyExists(course.Faculty) && course.Faculty != megaFaculty)
                    {
                        // checkTimeOfStudentGroup
                        if (CanAddStudent(student, course, threadNum))
                        {
                            // check if space is enough
                            if (course.CheckSpaceIfStudentIsAdded(threadNum))
                            {
                                // register
                                var threadForStudent = course.GetThread(threadNum);
                                student.Register(this, course, threadForStudent);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(
                            $"Megafaculty is empty or you tried to register student in the same megafaculty");
                    }
                }
            }
        }

        public Course FindCourse(string faculty)
        {
            faculty = faculty.ToLower();
            foreach (var course in Courses)
            {
                if (faculty == course.Faculty)
                {
                    return course;
                }
            }

            return null;
        }

        public void RemoveStudentFromCourse(string faculty, Student student)
        {
            faculty = faculty.ToLower();
            var course = FindCourse(faculty);
            course.RemoveStudent(student);
            student.Deregister(faculty);
        }

        public List<Thread> GetThreads(string faculty)
        {
            var course = FindCourse(faculty);
            if (course != null) return course.GetThreads();
            return null;
        }

        public List<Student> GetStudentOfThread(string faculty, int threadNum)
        {
            var course = FindCourse(faculty);
            return course.GetThread(threadNum).Students;
        }

        public List<Student> GetNonSubscribedStudents()
        {
            List<Student> allStudents = new List<Student>();
            var amountOfCourses = Enum.GetNames(typeof(CourseNumber)).Length;

            // get all students
            for (int i = 0; i < amountOfCourses; i++)
            {
                allStudents = allStudents.Union(FindStudents((CourseNumber)i + 1)).ToList();
            }

            if (MegaFaculty.Instance != null)
            {
                var amountOfOGNPCourses = MegaFaculty.Instance.Faculties.Count;
#pragma warning disable SA1312
                var OGNPstudents = new List<Student>();
#pragma warning restore SA1312
                for (int i = 0; i < amountOfOGNPCourses; i++)
                {
                    var threads = GetThreads(MegaFaculty.Instance.Faculties[i]);
                    if (threads == null) continue;
                    foreach (var thread in threads)
                    {
                        OGNPstudents = OGNPstudents.Union(thread.Students).ToList();
                    }
                }

                var nonIntersecting = allStudents.Union(OGNPstudents).Except(allStudents.Intersect(OGNPstudents))
                    .ToList();
                return nonIntersecting;
            }

            return null;
        }

        public void UpdateCourse(Course course, Thread thread)
        {
            foreach (var course1 in Courses)
            {
                if (course1.Faculty == course.Faculty)
                {
                    course1.UpdateThreadOfStudents(thread);
                }
            }
        }

        private bool IsGroupExists(GroupID id, OgnpService manager)
        {
            var group = HasGroup(id, manager);
            if (group == null) return false;
            return true;
        }

        private Group HasGroup(GroupID id, OgnpService manager)
        {
            CourseNumber courseNum = id.CourseNum;
            var numberOfGroup = id.Num;

            if (manager.DataOfGroupes[(int)courseNum].Count == 0) return null;
            var group = manager.DataOfGroupes[(int)courseNum].FirstOrDefault(t => t.GroupInfo.Num == numberOfGroup);
            return group;
        }

        private bool CanAddStudent(Student student, Course course, int threadNum)
        {
            var threadForStudent = course.GetThread(threadNum);

            if (threadForStudent == null) throw new Exception($"there is no thread with number{threadNum}");

            // check schedule
            var studentsGroup = FindGroup(student.GroupId);
            var threadLessons = threadForStudent.Lessons;
            foreach (var studentsGroupLesson in studentsGroup.Lessons)
            {
                foreach (var threadLesson in threadLessons)
                {
                    if (threadLesson.IsIntersect(studentsGroupLesson))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<Student> GetStudentsOfCourse(CourseNumber courseNumber)
        {
            List<Student> students = new List<Student>();
            var currCourse = DataOfGroupes[(int)courseNumber];
            foreach (Group tGroup in currCourse)
            {
                students = students.Union(tGroup.Students).ToList();
            }

            return students;
        }
    }
}