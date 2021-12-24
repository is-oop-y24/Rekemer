using System.Collections.Generic;
using ServerApplication.Common.Models;

namespace ServerApplication.DataLayer.Repository
{
    public abstract class DataContext
    {
        public DataToStore DataToStore { get; set; } = new DataToStore();
        public abstract void LoadTasks();
        public abstract void LoadWorkers();
        public abstract void Save();

        public List<T> Set<T>()
        {
            if (typeof(T) == typeof(Worker))
            {
                return DataToStore.Workers as List<T>;
            }

            if (typeof(T) == typeof(Task))
            {
                return DataToStore.Tasks as List<T>;
            }

            return null;
        }
    }
}