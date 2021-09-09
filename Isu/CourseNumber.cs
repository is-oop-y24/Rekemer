namespace Isu
{
    public class CourseNumber
    {
        private readonly int _courseNum;

        public CourseNumber(int courseNum)
        {
            _courseNum = courseNum;
        }

        public int CourseNum { get { return _courseNum; } }

        public static implicit operator CourseNumber(int num)
        {
            return new CourseNumber(num);
        }public static implicit operator int (CourseNumber num)
        {
            return num.CourseNum;
        }
    }
}