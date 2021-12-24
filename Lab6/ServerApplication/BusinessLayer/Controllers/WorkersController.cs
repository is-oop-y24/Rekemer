using System;
using System.Collections.Generic;
using ServerApplication.Common.Models;
using ServerApplication.DataLayer.Repository;

namespace ServerApplication.BusinessLayer.Controllers
{
    public class WorkersController
    {
        private Workers _workers;
        private Worker _teamLead;

        public WorkersController(DataContext dataContext)
        {
            _workers = new Workers(dataContext);
        }

        public void AddWorker(Worker leader, Worker worker = null)
        {
            if (leader is TeamLead)
            {
                _teamLead = leader;
            }

            if (leader != null && _workers.Entities.Find(t => t.ID == leader.ID) == null)
            {
                _workers.Entities.Add(leader);
            }

            if (worker != null && _workers.Entities.Find(t => t.ID == worker.ID) == null)
            {
                _workers.Entities.Add(worker);
            }

            if (leader != null)
            {
                if (worker != null && leader.IsLeader)
                {
                    leader.Subordinates.Add(worker);
                    worker.Leader = leader;
                }
            }
        }

        public List<Task> GetAllTaskOfWorker(string id)
        {
            var entities = GetWorkerById(id);
            if (entities == null)
            {
                throw new Exception("There is no such worker");
            }

            return entities.AssignedTasks;
        }

        public List<Worker> GetWorkers(bool isLeader = false)
        {
            if (isLeader == false)
            {
                return _workers.Entities;
            }

            return _workers.Entities.FindAll(t => t.IsLeader == isLeader);
        }

        public Worker GetWorkerById(string id)
        {
            return _workers.Entities.Find(t => t.ID == id);
        }

        public void Delete(Worker worker)
        {
            _workers.Delete(worker);
        }

        public void Create(Worker worker)
        {
            _workers.Create(worker);
        }

        public void Save()
        {
            _workers.Save();
        }

        public void Load()
        {
            _workers.Load();
        }

        public List<Report> GetAllReports()
        {
            List<Report> reports = new List<Report>();
            foreach (var worker in _workers.Entities)
            {
                if (worker.Report != null)
                {
                    reports.Add(worker.Report);
                }
            }

            return reports;
        }

        public void UpdateHierarchy()
        {
            if (_teamLead != null)
            {
                foreach (var worker in _workers.Entities)
                {
                    if (worker.IsLeader == true && worker.Leader == null && worker.ID != _teamLead.ID)
                    {
                        worker.Leader = _teamLead;
                        _teamLead.Subordinates.Add(worker);
                    }
                }
            }
        }

        public List<Worker> FindWorkersWithDoneReports()
        {
            List<Worker> workers = _workers.Entities.FindAll(t => t.Report.ReportStatus == ReportStatus.Saved);
            return workers;
        }

        public List<Worker> FindWorkersWithNotDoneReports()
        {
            List<Worker> workers = _workers.Entities.FindAll(t => t.Report.ReportStatus != ReportStatus.Saved);
            return workers;
        }
    }
}