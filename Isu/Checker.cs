using System.Linq;

namespace Isu
{
    class Checker
    {
        public bool CheckIfGroupExists(GroupID id, GroupManager manager)
        {
            var group = HasGroup(id, manager);
            if (group == null) return false;
            return true;
        }

        public Group HasGroup(GroupID id, GroupManager manager)
        {
            CourseNumber courseNum = id.courseNum;
            var numberOfGroup = id.num;

            if (manager.dataOfGroupes[courseNum].Count == 0) return null;
            var group = manager.dataOfGroupes[courseNum].FirstOrDefault(t => t.GroupInfo.num == numberOfGroup);
            return group;
        }
    }
}