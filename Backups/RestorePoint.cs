using System;
using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class RestorePoint
    {
        private DateTime _time;
        private string _directory;
        private List<FileInfo> _files;

        public List<FileInfo> Files
        {
            get
            {
                return new List<FileInfo>(_files);
            }
        }

        public RestorePoint(DateTime time, string directory, List<FileInfo> files)
        {
            this._time = time;
            this._directory = directory;
            this._files = files;
        }
    }
}