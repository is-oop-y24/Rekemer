using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backups
{
    public class Job
    {
        private IRepository repository;
        private List<FileInfo> filesToSave;
        private Algorithm _algorithm;

        public Job(IRepository repository, List<FileInfo> filesToSave, Algorithm algorithm)
        {
            this.repository = repository;
            this.filesToSave = filesToSave;

            _algorithm = algorithm;
        }

        public RestorePoint Launch()
        {
            if (_algorithm == Algorithm.singleStorage)
            {
                string path = SingleStorageSave(filesToSave);
                path = Path.GetDirectoryName(path);
                if (path == null) return null;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                RestorePoint point = new RestorePoint(DateTime.Now, path, directoryInfo.GetFiles().ToList());
                return point;
            }
            else if (_algorithm == Algorithm.splitStorage)
            {
                var files = SplitStorageSave(filesToSave);
                string path = null;
                foreach (var fileInfo in files)
                {
                    if (fileInfo != null)
                    {
                        path = fileInfo.DirectoryName;
                        break;
                    }
                }

                RestorePoint restorePoint = new RestorePoint(DateTime.Now, path, files);
                return restorePoint;
            }

            return null;
        }

        public List<FileInfo> SplitStorageSave(List<FileInfo> files)
        {
            List<FileInfo> names = new List<FileInfo>();
            foreach (var fileInfo in files)
            {
                string name = fileInfo.Name;
                names.Add(new FileInfo(repository.CreateZipCopyOfFile(fileInfo)));
            }

            return names;
        }

        string SingleStorageSave(List<FileInfo> files)
        {
            string directory = repository.AddFilesToArchive(files);
            if (directory != null)
            {
                return directory;
            }

            return null;
        }
    }
}