using System;
using System.Collections.Generic;
using System.IO;
using BackupsExtra.Algorithms.SaveAlgorithms;

namespace BackupsExtra.Jobs
{
    [Serializable]
    public class Job
    {
        string _repositoryPath;


        List<string> _filesToSave;
        private string _algorithmName;
        [NonSerialized] IAlgorithm _algorithm;

        public Job()
        {
        }

        public Job(string repositoryPath, List<string> filesToSave, IAlgorithm algorithm)
        {
            this._repositoryPath = repositoryPath;
            this._filesToSave = filesToSave;

            _algorithm = algorithm;
            _algorithmName = _algorithm.NameOfAlgorithm();
        }

        public RestorePoint Launch()
        {
            var files = _algorithm.Operation(_filesToSave, new Repository(_repositoryPath));
            if (files == null) return null;
            var directoryName = Path.GetDirectoryName(files[0]);
            if (directoryName != null)
            {
                RestorePoint restorePoint = new RestorePoint(Time.Instance.CurrentTime.ToString(), files,
                    _algorithmName, _filesToSave);
                return restorePoint;
            }


            return null;
        }
    }
}