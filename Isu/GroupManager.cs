using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    public class GroupManager
    {
        public List<Group>[] dataOfGroupes;

        public GroupManager()
        {
            dataOfGroupes = new List<Group>[10];
        }

        public void CreateGroup(GroupID id )
        {
            Group group = new Group(id.courseNum, id.num);
            dataOfGroupes[id.courseNum].Add(group);
        }
        public bool checkIfGroupExists(GroupID id)
        {
            var group = HasGroup(id);
            if (group == null) return false;
            return true;
        }
       
        private Group HasGroup(GroupID id)
        {
            CourseNumber courseNum = id.courseNum;
            var numberOfGroup = id.num;
            if (dataOfGroupes[courseNum] == null) dataOfGroupes[courseNum] = new List<Group>();
            if (dataOfGroupes[courseNum].Count == 0) return null;
            var group = dataOfGroupes[courseNum].FirstOrDefault(t => t.groupInfo.num == numberOfGroup);
            return group;
        }

        public void RegisterAStudent(Group group, Student student)
        {
            var existingGroup = HasGroup(group.groupInfo);
            if(existingGroup == null) throw new IsuException($"group{StringProccessor.FormAName(group.groupInfo)} doesn't exist");
           existingGroup.AddingAStudent(student);
        }

        public Student FindById(int id)
        {
            Student student = null;
            student = FindByParametr(id, student);

            return student;
        }

        private Student FindByParametr<T>(T id, Student student)
        {
            
            for (int i = 0; i < dataOfGroupes.Length; i++)
            {
                if (dataOfGroupes[i] == null) continue;
                for (int j = 0; j < dataOfGroupes[i].Count; j++)
                {
                    var groups = dataOfGroupes[i];
                    for (int k = 0; k < groups.Count; k++)
                    {
                        student = groups[k].students.FirstOrDefault(t => ( id is int ? Equals(t.id, id) : Equals(t.Name, id)  ));
                    }
                }
            }

            return student;
        }

        public Student FindByName (string name) {
            Student student = null;
            student = FindByParametr(name, student);

            return student;
        }

        public List<Student> GetStudentsOfgroup(string nameOfgroup)
        {
            GroupID groupid = StringProccessor.ParseName(nameOfgroup);
            Group group = dataOfGroupes[groupid.courseNum].FirstOrDefault( t=> t.Name == nameOfgroup);
            if(group == null) throw  new IsuException($"{nameOfgroup} is empty");
            return group.students;
            
        }

        public List<Student> GetStudentsOfCourse(CourseNumber courseNumber)
        {
            List<Student> students = new List<Student>();
            var currCourse = dataOfGroupes[courseNumber];
            foreach (Group tGroup in currCourse)
            {
                students.AddRange(tGroup.students);
            }

            if (students.Count == 0) throw new IsuException($"There are no students on this {courseNumber} course");
            return students;
        }

        public Group GetGroup(string groupName)
        {

            GroupID id = StringProccessor.ParseName(groupName);
            var group = dataOfGroupes[id.courseNum].FirstOrDefault(t => t.groupInfo.num == id.num);
            if (group == null) throw new IsuException($"There is no group {groupName}");
            return group;
        }

        public List<Group> GetGroups(CourseNumber courseNumber)
        {
            var groups = dataOfGroupes[courseNumber];
            if (groups == null) throw new IsuException($"There are no groups in course{courseNumber}");
            return groups;
        }

        public void DeregisterAStudent(Student student)
        {
            GroupID groupid = student.studentsGroup;
            Group group = GetGroup(StringProccessor.FormAName(groupid));
            if (group == null) throw new IsuException("There is no group in which this student exists");
            
            group.DeleteStudent(student);

        }
    }
    
}