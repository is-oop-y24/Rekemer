using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    public class GroupSearcher
    {
        public Group GetGroup(string groupName, GroupManager manager)
        {
            GroupID id = StringProccessor.ParseName(groupName);
            var group = manager.dataOfGroupes[id.courseNum].FirstOrDefault(t => t.GroupInfo.num == id.num);
            if (group == null) throw new IsuException($"There is no group {groupName}");
            return group;
        }

        public List<Group> GetGroups(CourseNumber courseNumber, GroupManager manager)
        {
            var groups = manager.dataOfGroupes[courseNumber];
            if (groups == null) throw new IsuException($"There are no groups in course{courseNumber}");
            return groups;
        }
    }
}