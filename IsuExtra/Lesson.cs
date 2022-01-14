namespace IsuExtra
{
    public enum DaysOfWeek
    {
#pragma warning disable SA1602
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
#pragma warning restore SA1602
    }

    public class Lesson
    {
        public Lesson(string teacher, DaysOfWeek day, TimeInterval time, int auiditory)
        {
            this.Teacher = teacher;
            this.Day = day;
            this.Time = time;
            this.Auiditory = auiditory;
        }

        public string Teacher { get; private set; }
        public DaysOfWeek Day { get; private set; }
        public TimeInterval Time { get; private set; }
        public int Auiditory { get; private set; }

        public bool IsIntersect(Lesson lesson)
        {
            if (Day != lesson.Day) return true;
            return Time.IsIntersect(lesson.Time);
        }
    }
}