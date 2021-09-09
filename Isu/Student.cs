using System;

namespace Isu
{
    public class Student
    {
        public GroupID studentsGroup { get; private set; }
        public int id { get; private set; }
        public string Name { get; private set; }


        private Student()
        {
            
        }

        public Student( string name,Group group)
        {
            Name = name;
            studentsGroup = new GroupID(group.groupInfo.courseNum, group.groupInfo.num);
            id = StringProccessor.GetID(name);
        }
    }
}