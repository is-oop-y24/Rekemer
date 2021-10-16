using Isu.Tools;
using System;

namespace Isu
{
    public class GroupID
    {
        public readonly CourseNumber courseNum;
        public readonly int num;
        public string Name { get; private set; }

        public GroupID(GroupID groupId)
        {
            this.courseNum = groupId.courseNum;
            this.num = groupId.num;
            this.Name  = groupId.Name ;
        }
        private GroupID(CourseNumber courseNum, int num)
        {
            this.courseNum = courseNum;
            this.num = num;
            Name = FormAName(courseNum, num);
        }

        public static implicit operator string(GroupID id)
        {
            return id.Name;
        }
        public static implicit operator GroupID(string name)
        {
            GroupID id = ParseName(name);
            return id;
        }
        public static  GroupID ParseName(string name)
        {
            int number;
            string numSubstring = name.Substring(1);
            bool areInts = int.TryParse(numSubstring, out number);
            var myEnumMemberCount = Enum.GetNames(typeof(CourseNumber)).Length;
            char courseNum = name[2];
            int intCourseNum = courseNum - '0';
            bool isCourseNumberExists = intCourseNum < myEnumMemberCount;
            if (name.Length != 5 || name[0] != 'M' ||  !isCourseNumberExists || !areInts)
                throw new IsuException("Invalid group name (M3XYY)");

            GroupID groupInfo = new GroupID((CourseNumber)int.Parse(name[2].ToString()), byte.Parse(numSubstring.Substring(2)));
            return groupInfo;
        }

        public static string FormAName(CourseNumber courseNum, int groupNumber )
        {
            int course = (int)courseNum;
            string name = "M3" + course.ToString();
            if (groupNumber < 10) name += "0" + groupNumber.ToString();
            else name += groupNumber.ToString();
            return name;
        }

        
    }
}