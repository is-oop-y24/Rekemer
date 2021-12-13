using System;
using System.IO;

namespace BackupsExtra.Algorithms.LogInterface
{
    public class FileLog : ILog
    {
        private string _pathOfFile;

        public FileLog(string pathOfFile)
        {
            _pathOfFile = pathOfFile;
        }

        public void Log(string message)
        {
            string createText = LogInterface.Log.IsTimePrefix
                ? DateTime.Now.ToString() + " " + message + Environment.NewLine
                : message + Environment.NewLine;
            File.AppendAllText(_pathOfFile, createText);
        }
    }
}