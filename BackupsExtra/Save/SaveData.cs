using System;
using System.Collections.Generic;
using BackupsExtra.Algorithms.SaveAlgorithms;
using BackupsExtra.Jobs;

namespace BackupsExtra.Save
{
    [Serializable]
    public class SaveData
    {
        private List<Job> _jobs = new List<Job>();
        [NonSerialized]
        private IAlgorithm _algorithm;
        private string _repository;
#pragma warning disable SA1214
        private readonly List<RestorePoint> _restorePoints;
#pragma warning restore SA1214

        public SaveData()
        {
        }

        public SaveData(List<Job> jobs, string repository, IAlgorithm algorithm, List<RestorePoint> restorePoints)
        {
            this._jobs = jobs;
            _repository = repository;
            _algorithm = algorithm;
            _restorePoints = restorePoints;
        }

        public List<RestorePoint> RestorePoints
        {
            get => _restorePoints;
        }

        public List<Job> Jobs
        {
            get => _jobs;
        }

        public IAlgorithm Algorithm
        {
            get => _algorithm;
        }

        public string Repository
        {
            get => _repository;
        }
    }
}