using System;

namespace Isu
{
    public class Student
    {
        public GroupID studentsGroup { get; private set; }
        public int id { get; private set; }
        public string name { get; private set; }


        private Student()
        {
            
        }

        public Student( string name,Group group)
        {
            this.name = name;
            studentsGroup = new GroupID(group.GroupInfo.courseNum, group.GroupInfo.num);
            id = StringProccessor.GetID(name);
        }
    }
}