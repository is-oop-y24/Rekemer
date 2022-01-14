using System;
using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class Job
    {
        private IRepository repository;
        private List<FileInfo> filesToSave;
        private IAlgorithm _algorithm;

        public Job(IRepository repository, List<FileInfo> filesToSave, IAlgorithm algorithm)
        {
            this.repository = repository;
            this.filesToSave = filesToSave;
            _algorithm = algorithm;
        }

        public RestorePoint Launch()
        {
            var files = _algorithm.Operation(filesToSave, repository);
            if (files == null) return null;
            var directoryName = files[0].DirectoryName;
            if (directoryName != null)
            {
                string path = directoryName.ToString();
                RestorePoint restorePoint = new RestorePoint(DateTime.Now, path, files);
                return restorePoint;
            }

            return null;
        }
    }
}