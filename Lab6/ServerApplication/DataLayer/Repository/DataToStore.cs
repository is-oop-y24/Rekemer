using System.Collections.Generic;
using ServerApplication.Common.Models;

namespace ServerApplication.DataLayer.Repository
{
    public class DataToStore
    {
        public List<Worker> Workers { get; set; } = new List<Worker>();
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}