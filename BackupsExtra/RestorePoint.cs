using System;
using System.Collections.Generic;

namespace BackupsExtra
{
    [Serializable]
    public class RestorePoint
    {
        public readonly string _time;
        
        private List<string> _files;
        public readonly string Alghoritm;
        private List<string> _originalFiles ;
        public List<string> Files
        {
            get { return _files; }
            set { _files = value; }
        }
        public List<string> OriginalFiles
        {
            get { return _originalFiles; }
            set { _originalFiles = value; }
        }
    public RestorePoint(string time,  List<string> files, string alghoritm, List<string> originalFiles)
    {
    this._time = time;
    this._files = new List<string>(files) ;
    this.Alghoritm = alghoritm;
    _originalFiles = new List<string>(originalFiles);
    }
}

}