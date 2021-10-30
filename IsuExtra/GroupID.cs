using Isu.Tools;
using System;


namespace IsuExtra
{
    public class GroupID
    {
        public readonly CourseNumber CourseNum;
        public readonly int Num;

        public readonly MegaFaculty Faculty;

        //private PrefixOfName _prefixOfName;
        public string Name { get; private set; }

        public GroupID(GroupID groupId)
        {
            this.CourseNum = groupId.CourseNum;
            this.Num = groupId.Num;
            this.Name = groupId.Name;
        }

        private GroupID(CourseNumber courseNum, int num, MegaFaculty megaFaculty)
        {
            this.CourseNum = courseNum;
            this.Num = num;
            Faculty = megaFaculty;
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

        public static GroupID ParseName(string name)
        {
            int number;
            string numSubstring = name.Substring(1);
            bool areInts = int.TryParse(numSubstring, out number);
            var myEnumMemberCount = Enum.GetNames(typeof(CourseNumber)).Length;
            char courseNum = name[2];
            int intCourseNum = courseNum - '0';
            bool isCourseNumberExists = intCourseNum < myEnumMemberCount;
            MegaFaculty megaFaculty;
            if (name.Length != 5 || name[0] != 'C' && name[0] != 'T' && name[0] != 'B' && name[0] != 'S' ||
                !isCourseNumberExists || !areInts)
                throw new IsuException("Invalid group name (K3XYY)");
            switch (name[0])
            {
                case 'C':
                    megaFaculty = MegaFaculty.CompTech;
                    break;
                case 'T':
                    megaFaculty = MegaFaculty.TranslInf;
                    break;
                case 'B':
                    megaFaculty = MegaFaculty.BioTech;
                    break;
                case 'S':
                    megaFaculty = MegaFaculty.SciLife;
                    break;
                default:
                    megaFaculty = MegaFaculty.None;
                    throw new Exception($"Groups with prefix {name[0]} do not exist");
            }

            GroupID groupInfo = new GroupID((CourseNumber) int.Parse(name[2].ToString()),
                byte.Parse(numSubstring.Substring(2)), megaFaculty);
            return groupInfo;
        }
    }
}