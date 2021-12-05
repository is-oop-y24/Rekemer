using System;

namespace BackupsExtra.Algorithms.LogInterface
{
    public class Log
    {
        private static ILog _log;
        public static bool IsTimePrefix { get; set; }
        public static ILog Instance
        {
            get
            {
                if (_log == null)
                {
                    throw new Exception("Init logger before accessing it");
                }

                return _log;
            }
        }

        public static void Init(ILog log)
        {
            _log = log;
        }
    }
}