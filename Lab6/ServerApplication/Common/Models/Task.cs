using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServerApplication.DataLayer.Repository;

namespace ServerApplication.Common.Models
{
    public class Task : IHaveID
    {
        [JsonProperty("CommentOfTask")] public string Comment { get; set; }
        [JsonProperty("StateOfTask")] public TaskState State { get; set; }
        [JsonProperty("LastTimeChanged")] public string LastTimeChanged { get; set; }
        [JsonProperty("AssignedWorkers")] public List<Worker> AssignedWorkers { get; set; } = new List<Worker>();
        [JsonProperty("TimeOfBirth")] public readonly string TimeOfBirth;
        [Newtonsoft.Json.JsonIgnore] private string _id = Guid.NewGuid().ToString();

        public Task()
        {
            _id = Guid.NewGuid().ToString();
            TimeOfBirth = DateTime.Now.ToString();
        }

        [JsonProperty("ID")]
        public string ID
        {
            get => _id;
            private set => _id = value;
        }
    }
}