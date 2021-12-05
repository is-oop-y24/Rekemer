using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Algorithms.DeleteAlgorithms;
using BackupsExtra.Algorithms.LogInterface;
using BackupsExtra.Algorithms.RestoreAlgorithms;
using BackupsExtra.Algorithms.SaveAlgorithms;
using BackupsExtra.Save;

namespace BackupsExtra.Jobs
{
    public class BackupJob
    {
        private List<RestorePoint> _restorePoints = new List<RestorePoint>();
        private IAlgorithm _algorithm;
        private IDeleteAlgorithm _deleteAlgorithm;
        private List<Job> _jobs = new List<Job>();
        private List<string> _filesToSave = new List<string>();
        private Repository _repository;


        public List<RestorePoint> RestorePoints
        {
            get { return _restorePoints; }
        }

        

        public IRepository Repository
        {
            get { return new Repository(_repository.DirectoryInfo); }
        }

        public BackupJob()
        {
            
        }

        public BackupJob(Repository repository,IAlgorithm algorithm,   IDeleteAlgorithm deleteAlgorithm)
        {
            this._repository = repository;
            _algorithm = algorithm;
            
            
            _deleteAlgorithm = deleteAlgorithm;
            
        }

        public void AddFiles(List<string> fileInfos)
        {
            _filesToSave = _filesToSave.Union(fileInfos).ToList();
            // add in log
            string message = AddedFiles(fileInfos);
            Log.Instance.Log(message);
        }

        private string AddedFiles(List<string> fileInfos)
        {
            string message="FilesToSave are added to BackupJob" + Environment.NewLine;
            foreach (var fileInfo in fileInfos)
            {
                message += fileInfo + Environment.NewLine;
            }

            return message;
        }
        
        private string RestorePointIsAdded(RestorePoint restorePoint)
        {
            string message = "job is added" + Environment.NewLine;
            var time = restorePoint._time;
            var alghoritm = restorePoint.Alghoritm;
            var files = restorePoint.Files;
            message += "Time: " + time + " algorithm: " + alghoritm + "files: " + files;
            return message;
        }
        private string SystemIsSaved()
        {
            string message = "system is saved in ";
            return message;
        }

        
        public void RemoveFile(string fileInfo)
        {
            var file = _filesToSave.First(t => t == fileInfo);
            _filesToSave.Remove(file);
        }

        public void SetAlghoritm(IAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }
        public void SetDeleteAlghoritm(IDeleteAlgorithm algorithm)
        {
            _deleteAlgorithm = algorithm;
        }
        public void Save()
        {
            Job job = new Job(_repository.DirectoryInfo, _filesToSave, _algorithm);
            RestorePoint restorePoint = job.Launch();
           
            // add in log
            string message = RestorePointIsAdded(restorePoint);
            Log.Instance.Log(message);
            if (restorePoint != null)
            {
                _restorePoints.Add(restorePoint);
            }
            _jobs.Add(job);
            SaveData saveData = new SaveData(_jobs,_repository.DirectoryInfo,_algorithm,_restorePoints );
            SaveSystem.Save(saveData);
            _deleteAlgorithm.Delete( ref _restorePoints);
           
        }

        public bool Load()
        {
            var saveData = SaveSystem.Load();
            if (saveData != null)
            {
                _jobs = saveData.Jobs;
                _repository = new Repository( saveData.Repository);
                _algorithm = saveData.Algorithm;
                _restorePoints = saveData.RestorePoints;
                
                return true;
            }
            return false;
            
        }
        
        public void Restore(RestorePoint restorePoint, Restore restore)
        {
                restore.RestoreFiles(restorePoint);
        }
    }
}