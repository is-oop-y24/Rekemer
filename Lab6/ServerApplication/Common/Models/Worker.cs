using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServerApplication.Common.Commands;
using ServerApplication.DataLayer.Repository;

namespace ServerApplication.Common.Models
{
    public class Worker : IHaveID
    {
        private Report _report;

        public Report Report
        {
            get => _report;
            set => _report = value;
        }

        [JsonProperty("IsLeader")] public bool IsLeader { get; set; }

        [Newtonsoft.Json.JsonIgnore] private string _id = Guid.NewGuid().ToString();

        [JsonProperty("ID")]
        public string ID
        {
            get => _id;
            private set => _id = value;
        }

        [JsonProperty("AssignedTasksString")] public string AssignedTasksStrings { get; set; }
        [JsonProperty("ActionsOnTasksString")] public string ActionsOnTasksString { get; set; }

        [JsonIgnore] public List<Task> AssignedTasks { get; set; } = new List<Task>();
        [JsonIgnore] public List<Command> ActionsOnTasks { get; set; } = new List<Command>();
        [JsonIgnore] public List<Worker> Subordinates { get; set; } = new List<Worker>();
        [JsonProperty("leaderWorker")] private Worker _leader;
        [JsonProperty("Name")] public string Name { get; set; }

        public Worker(string name, bool isLeader = false)
        {
            Name = name;
            IsLeader = isLeader;
        }

        public Worker()
        {
        }

        [JsonIgnore]
        public Worker Leader
        {
            get => _leader;
            set
            {
                if (value != null && value.IsLeader)
                {
                    _leader = value;
                    _leader.Subordinates = value.Subordinates;
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        public virtual bool IsTeamLead()
        {
            return false;
        }
    }
}