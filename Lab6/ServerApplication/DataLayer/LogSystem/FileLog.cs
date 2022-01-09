using System;
using System.IO;

namespace ServerApplication.DataLayer.LogSystem
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
            string createText = DateTime.Now.ToString() + " " + message + Environment.NewLine;

            File.AppendAllText(_pathOfFile, createText);
        }

        public string FindTimesOfParticularAction(string nameOfAction)
        {
            string searchText = nameOfAction;
            string old;
            string times = "";
            StreamReader sr = File.OpenText(_pathOfFile);
            while ((old = sr.ReadLine()) != null)
            {
                if (old.Contains(searchText))
                {
                    times += old + Environment.NewLine;
                }
            }

            sr.Close();
            return times;
        }
    }
}