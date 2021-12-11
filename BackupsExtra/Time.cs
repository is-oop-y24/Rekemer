using System;

namespace BackupsExtra
{
    public class Time
    {
        private static Time _instance;
        private DateTime _dateTime;

        public static Time Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Time();
                    _instance._dateTime = DateTime.Now;
                }

                return _instance;
            }
        }

        public DateTime CurrentTime
        {
            get => _dateTime;
        }

        public void ResetTime()
        {
            _instance._dateTime = DateTime.Now;
        }

        public void AddTime(int years, int months, int days)
        {
            // add time
            _dateTime = _dateTime.AddYears(years).AddMonths(months).AddDays(days);
        }
    }
}