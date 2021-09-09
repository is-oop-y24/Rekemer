using Isu.Tools;

namespace Isu
{
    static class StringProccessor
    {
        public static GroupID ParseName(string name)
        {
            int number;
            string numSubstring = name.Substring(1);
            bool areInts = int.TryParse(numSubstring, out number);
            if (name.Length != 5 || name[0] != 'M' || name[1] !='3' || !areInts) throw new IsuException("Invalid group name (M3XYY)");

            GroupID groupInfo = new GroupID(int.Parse(name[2].ToString()), byte.Parse(numSubstring.Substring(2)));
            return groupInfo;
        }
        public  static string FormAName(GroupID groupId)
        {
            int course = groupId.courseNum;
            string name = "M3" + course.ToString();
            if (groupId.num < 10) name += "0" + groupId.num.ToString();
            else name += groupId.num.ToString();
            return name;
        }
        public static int GetID(string s)
        {
            int total = 0;
            char[] c;
            c = s.ToCharArray();
  
            
            for (int k = 0; k <= c.GetUpperBound(0); k++)
                total += (int)c[k];
  
            return total;
        }
    }
}