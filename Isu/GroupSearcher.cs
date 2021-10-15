using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    public class GroupSearcher
    {
        public Group GetGroup( GroupID id, GroupManager manager)
        {
           
            var group = manager.dataOfGroupes[id.courseNum].FirstOrDefault(t => t.GroupInfo.num == id.num);
            if (group == null) throw new IsuException($"There is no group {id.Name}");
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