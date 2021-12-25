using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServerApplication.Common.Models
{
    public class Report
    {
        [JsonProperty("Id")] public string Id { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("ReportStatus")] public ReportStatus ReportStatus { get; set; } = ReportStatus.Edit;
        [JsonProperty("DescriptionOfReport")] public string Description { get; set; }
        [JsonProperty("TasksOfReport")] public string TasksString { get; set; }

        [JsonIgnore] public List<Task> Tasks { get; set; } = new List<Task>();


        public Report(string description)
        {
            Description = description;
        }
    }
}