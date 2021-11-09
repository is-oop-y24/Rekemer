using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backups
{
    public enum Algorithm
    {
        singleStorage,
        splitStorage
    }

    public class BackupJob
    {
        private List<RestorePoint> restorePoints = new List<RestorePoint>();
        private IAlgorithm _algorithm;

        public List<RestorePoint> RestorePoints
        {
            get { return new List<RestorePoint>(restorePoints); }
        }

        private IRepository repository;

        public IRepository Repository
        {
            get { return new Repository(repository.DirectoryInfo.FullName); }
        }

        private List<FileInfo> filesToSave = new List<FileInfo>();

        public BackupJob(IRepository repository)
        {
            this.repository = repository;
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