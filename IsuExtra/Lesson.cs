namespace IsuExtra
{
    public enum DaysOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class Lesson
    {
        public string Teacher { get; private set; }
        public DaysOfWeek Day { get; private set; }
        public TimeInterval Time { get; private set; }
        public int Auiditory { get; private set; }

        public Lesson(string teacher, DaysOfWeek day, TimeInterval time, int auiditory)
        {
            this.Teacher = teacher;
            this.Day = day;
            this.Time = time;
            this.Auiditory = auiditory;
        }

        public bool IsIntersect(Lesson lesson)
        {
            if (Day != lesson.Day) return true;
            return Time.IsIntersect(lesson.Time);
        }
    }
}