using System;

namespace ServerApplication.DataLayer.LogSystem
{
    public class Log
    {
        private static ILog _log;


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