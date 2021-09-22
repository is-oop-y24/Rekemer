using Isu.Tools;

namespace Isu
{
    class Register
    {
        public void RegisterAStudent(Group group, Student student, GroupManager manager, Checker checker)
        {
            var existingGroup = checker.HasGroup(group.GroupInfo, manager);
            if (existingGroup == null)
                throw new IsuException($"group{StringProccessor.FormAName(group.GroupInfo)} doesn't exist");
            existingGroup.AddingAStudent(student);
        }

        public void DeregisterAStudent(Student student, GroupSearcher groupSearcher, GroupManager manager)
        {
            GroupID groupid = student.studentsGroup;
            Group group = groupSearcher.GetGroup(StringProccessor.FormAName(groupid), manager);
            if (group == null) throw new IsuException("There is no group in which this student exists");

            group.DeleteStudent(student);
        }
    }
}