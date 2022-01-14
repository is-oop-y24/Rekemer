using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups
{
    public class BackupJob
    {
        private List<RestorePoint> restorePoints = new List<RestorePoint>();
        private IAlgorithm _algorithm;
        private IRepository repository;
        private List<FileInfo> filesToSave = new List<FileInfo>();
        public BackupJob(IRepository repository, IAlgorithm algorithm)
        {
            this.repository = repository;
            _algorithm = algorithm;
        }

        public List<RestorePoint> RestorePoints
        {
            get { return new List<RestorePoint>(restorePoints); }
        }

        public IRepository Repository
        {
            get { return new Repository(repository.DirectoryInfo.FullName); }
        }

        public void AddFiles(List<FileInfo> fileInfos)
        {
            filesToSave = filesToSave.Union(fileInfos).ToList();
        }

        public void RemoveFile(FileInfo fileInfo)
        {
            var file = filesToSave.First(t => t.Name == fileInfo.Name);
            filesToSave.Remove(file);
        }

        public void SetAlghoritm(IAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public void Save()
        {
            Job job = new Job(repository, filesToSave, _algorithm);
            RestorePoint restorePoint = job.Launch();
            if (restorePoint != null)
            {
                restorePoints.Add(restorePoint);
            }
        }
    }
}