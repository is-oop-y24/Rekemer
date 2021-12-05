using System;

namespace BackupsExtra.Algorithms.LogInterface
{
    public class ConsoleLog : ILog
    {
        public void Log(string message)
        {
            string createText = LogInterface.Log.IsTimePrefix
                ? DateTime.Now.ToString() + " " + message + Environment.NewLine
                : message + Environment.NewLine;
            Console.WriteLine(createText);
        }
    }
}