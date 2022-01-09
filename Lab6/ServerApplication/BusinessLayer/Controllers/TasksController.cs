using System;
using System.Collections.Generic;
using System.Linq;
using ServerApplication.Common.Commands;
using ServerApplication.Common.Models;
using ServerApplication.DataLayer.DataContext;
using ServerApplication.DataLayer.LogSystem;
using ServerApplication.DataLayer.Repository;

namespace ServerApplication.BusinessLayer.Controllers
{
    public class TasksController
    {
        private Tasks _tasks;

        public TasksController(DataContext dataContext)
        {
            _tasks = new Tasks(dataContext);
        }

        public List<Task> GetAllTasks()
        {
            return _tasks.Entities;
        }

        public void AddTask(Task task, Worker worker = null)
        {
            _tasks.Create(task);
            if (worker != null)
            {
                ChangeAssignedWorkerOfTask(task, worker);
            }
        }

        public List<Task> GetTasksWithLatestChanges()
        {
            var orderedByDateTasks = from task in _tasks.Entities
                orderby Convert.ToDateTime(task.LastTimeChanged)
                select task;

            return orderedByDateTasks.Reverse().ToList();
        }

        public List<Task> GetTaskWithThisDateTime(string date)
        {
            return _tasks.Entities.FindAll(t => Convert.ToDateTime(t.TimeOfBirth) == Convert.ToDateTime(date));
        }

        public Task GetTaskById(string id)
        {
            return _tasks.GetById(id);
        }


        public List<Task> GetTasksChangedByWorker(Worker worker)
        {
            var actions = worker.ActionsOnTasks;
            var tasks = new List<Task>();
            foreach (var action in actions)
            {
                tasks.Add(action.Task);
            }

            return tasks;
        }

        public List<Report> GetReportsOfSubordinates(Worker worker)
        {
            List<Report> reports = new List<Report>();
            if (worker.IsLeader)
            {
                foreach (Worker subordinate in worker.Subordinates)
                {
                    reports.Add(subordinate.Report);
                    reports = reports.Union(GetReportsOfSubordinates(subordinate)).ToList();
                }
            }

            return reports;
        }

        public void AddTaskToReport(Task task, Report report)
        {
            if (report.ReportStatus != ReportStatus.Edit) return;
            if (report.Tasks.Find(t => t.ID == task.ID) == null)
            {
                report.Tasks.Add(task);
            }
        }

        public void UpdateReportDescriprion(Report report, string description)
        {
            report.Description = description;
        }

        public bool SaveReport(Report report)
        {
            foreach (Task task in report.Tasks)
            {
                if (task.State != TaskState.Resolved)
                {
                    return false;
                }
            }

            report.ReportStatus = ReportStatus.Saved;
            return true;
        }

        public List<Task> GetAllTasksOfSubordinates(Worker worker)
        {
            List<Task> tasks = new List<Task>();

            if (worker.IsLeader)
            {
                foreach (Worker subordinate in worker.Subordinates)
                {
                    tasks = tasks.Union(subordinate.AssignedTasks).ToList();
                    tasks = tasks.Union(GetAllTasksOfSubordinates(subordinate)).ToList();
                }
            }

            return tasks;
        }


        public void AddCommentToTask(Task task, string commment, Worker worker)
        {
            Command command = new AddCommentCommand(task, commment);
            command.Execute();
            worker.ActionsOnTasks.Add(command);
            // notify log
            string message =
                $"Action: {command.NameOfAction()} added a comment by worker with id: {worker.ID} and name {worker.Name}";
            Log.Instance.Log(message);
        }

        public void ChangeStateOfTask(Task task, TaskState state, Worker worker)
        {
            var prevousState = task.State;
            Command command = new ChangeStateOfTask(task, state);
            command.Execute();
            worker.ActionsOnTasks.Add(command);
            // notify log
            string message =
                $"Action: {command.NameOfAction()} state of task with id {task.ID} is changed from {prevousState} to {task.State}";
            Log.Instance.Log(message);
        }


        public void ChangeAssignedWorkerOfTask(Task task, Worker worker,
            Worker oldWorker = null)
        {
            Command command = new ChangeAssignedWorkerCommand(task, worker, oldWorker);
            command.Execute();
            worker.ActionsOnTasks.Add(command);
            // notify log
            string message;
            if (oldWorker == null)
            {
                message =
                    $@"Action: {command.NameOfAction()} Added new worker with id: {worker.ID} and name {worker.Name} to task {task.ID}";
            }
            else
            {
                message =
                    $@"Action: {command.NameOfAction()}  worker  with id: {oldWorker.ID} and name {oldWorker.Name} is replaced by new worker with id: {worker.ID} and name {worker.Name} in task {task.ID}";
            }

            Log.Instance.Log(message);
        }

        public void AssignTaskToWorker(Task task, Worker worker)
        {
            if (worker != null && task != null)
            {
                var tasks = worker.AssignedTasks;
                foreach (var t in tasks)
                {
                    if (t.AssignedWorkers.Find(t => t.ID == task.ID) != null)
                    {
                        return;
                    }
                }

                if (task.AssignedWorkers.Find(t => t.ID == worker.ID) != null)
                {
                    return;
                }

                worker.AssignedTasks.Add(task);
                task.AssignedWorkers.Add(worker);
            }
        }

        public void AssignReport(Worker worker, Report report)
        {
            worker.Report = report;
        }

        public void Save()
        {
            _tasks.Save();
        }

        public void Load()
        {
            _tasks.Load();
        }

        public void DeleteWorkerFromTasks(string id)
        {
            var tasks = GetAllTasks();
            foreach (Task task in tasks)
            {
                task.AssignedWorkers.RemoveAll(k => k.ID == id);
            }
        }
    }
}