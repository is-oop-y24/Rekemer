using System;
using Isu.Tools;

namespace IsuExtra
{
    public class GroupID
    {
#pragma warning disable SA1401
        public readonly CourseNumber CourseNum;
        public readonly int Num;

        public readonly string Faculty;
#pragma warning restore SA1401

        public GroupID(GroupID groupId)
        {
            this.CourseNum = groupId.CourseNum;
            this.Num = groupId.Num;
            this.Name = groupId.Name;
        }

        private GroupID(CourseNumber courseNum, int num, string megaFaculty)
        {
            this.CourseNum = courseNum;
            this.Num = num;
            Faculty = megaFaculty;
        }

        public string Name { get; private set; }

        public static implicit operator string(GroupID id)
        {
            return id.Name;
        }

        public static implicit operator GroupID(string name)
        {
            GroupID id = ParseName(name);
            return id;
        }

        public static GroupID ParseName(string name)
        {
            int number;
            string numSubstring = name.Substring(1);
            bool areInts = int.TryParse(numSubstring, out number);
            var myEnumMemberCount = Enum.GetNames(typeof(CourseNumber)).Length;
            char courseNum = name[2];
            int intCourseNum = courseNum - '0';
            bool isCourseNumberExists = intCourseNum < myEnumMemberCount;
            if (MegaFaculty.Instance != null)
            {
                bool isTherePrefix = MegaFaculty.Instance.IsTherePrefix(name[0]);

                if (name.Length != 5 || !isTherePrefix ||
                    !isCourseNumberExists || !areInts)
                    throw new IsuException("Invalid group name (K3XYY)");
                string megaFaculty = MegaFaculty.Instance.GetMegafaculty(name[0]);
                if (megaFaculty != null)
                {
                    GroupID groupInfo = new GroupID(
                        (CourseNumber)int.Parse(name[2].ToString()),
                        byte.Parse(numSubstring.Substring(2)),
                        megaFaculty);
                    return groupInfo;
                }
            }

            return null;
        }
    }
}