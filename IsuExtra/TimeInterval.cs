using System.Numerics;

namespace IsuExtra
{
    public class TimeInterval
    {
        private Vector2 startTime;
        private Vector2 endTime;

        public TimeInterval(Vector2 startTime, Vector2 endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public bool IsIntersect(TimeInterval time)
        {
            var startTime1 = time.startTime;
            var endTime1 = time.endTime;

            if ((startTime.X < startTime1.X && startTime1.X < endTime.X) ||
                (startTime1.X < startTime.X && startTime.X < endTime1.X))
            {
                return true;
            }

            if (startTime1.X == endTime.X || startTime.X == endTime1.X)
            {
                if ((startTime.Y <= startTime1.Y && startTime1.Y <= endTime.Y) ||
                    (startTime1.Y <= startTime.Y && startTime.Y <= endTime1.Y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}