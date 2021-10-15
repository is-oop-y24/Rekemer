using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Isu
{
    public static class Builder
    {
        public static Group.GroupBuilder GroupBuilder => new Group.GroupBuilder();
        public static Student.StudentBuilder StudentBuilder => new Student.StudentBuilder();
        
    }
}
