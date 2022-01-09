using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ServerApplication.Common.Commands;
using ServerApplication.Common.Models;

namespace ServerApplication.DataLayer.DataContext
{
    public class JsonDataContext : DataContext
    {
        public static string PathOfSavingForWorkers { get; set; } = @"C:\lab5\workers.json";
        public static string PathOfSavingForTasks { get; set; } = @"C:\lab5\tasks.json";

        public override void Save()
        {
            SerializeTasks(DataToStore.Tasks);
            SerializeWorkers(DataToStore.Workers);
        }

        public override void LoadTasks()
        {
            DataToStore.Tasks = DeserializeTasks();
        }

        public override void LoadWorkers()
        {
            DataToStore.Workers = DeserializeWorkers();
        }

        public static void SerializeTasks(List<Task> tasks)
        {
            if (File.Exists(PathOfSavingForTasks))
            {
                File.WriteAllText(PathOfSavingForTasks, "");
                var json = JsonConvert.SerializeObject(tasks);

                File.AppendAllText(PathOfSavingForTasks, json);
            }
        }

        public static List<Task> DeserializeTasks()
        {
            if (File.Exists(PathOfSavingForTasks))
            {
                var tasks = new List<Task>();


                using (StreamReader r = new StreamReader(PathOfSavingForTasks))
                {
                    string json = r.ReadToEnd();
                    tasks = JsonConvert.DeserializeObject<List<Task>>(json);
                }

                var allWorkers = DeserializeWorkers();
                ResetSubordinatesAfterSerialization(allWorkers);

                UpdateAssignedWorkerAfterDeserialization(tasks, allWorkers);

                return tasks;
            }

            return null;
        }

        public static void UpdateAssignedWorkerAfterDeserialization(List<Task> tasks, List<Worker> allWorkers)
        {
            foreach (var task in tasks)
            {
                foreach (var assignedWorker in (task.AssignedWorkers))
                {
                    if (assignedWorker != null)
                    {
                        var idOfAssigned = assignedWorker.ID;
                        if (assignedWorker.IsLeader)
                        {
                            foreach (var walker in allWorkers)
                            {
                                if (walker.Leader != null && walker.Leader.ID == idOfAssigned)
                                {
                                    if (assignedWorker.Subordinates.Find(t => t.ID == walker.ID) == null)
                                    {
                                        assignedWorker.Subordinates.Add(walker);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SerializeWorkers(List<Worker> workers)
        {
            if (File.Exists(PathOfSavingForWorkers))
            {
                File.WriteAllText(PathOfSavingForWorkers, "");
                foreach (var worker in workers)
                {
                    worker.AssignedTasksStrings = JsonConvert.SerializeObject(worker.AssignedTasks,
                        Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });

                    worker.ActionsOnTasksString = JsonConvert.SerializeObject(worker.ActionsOnTasks,
                        Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                    if (worker.Report != null)
                    {
                        worker.Report.TasksString = JsonConvert.SerializeObject(worker.Report.Tasks,
                            Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });
                    }
                }


                var json = JsonConvert.SerializeObject(workers, Newtonsoft.Json.Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });


                File.AppendAllText(PathOfSavingForWorkers, json);
            }
        }

        public static List<Worker> DeserializeWorkers()
        {
            if (File.Exists(PathOfSavingForWorkers))
            {
                var workers = new List<Worker>();


                using (StreamReader r = new StreamReader(PathOfSavingForWorkers))
                {
                    string json = r.ReadToEnd();
                    workers = JsonConvert.DeserializeObject<List<Worker>>(json, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }


                ResetSubordinatesAfterSerialization(workers);
                UpdateActionsAndAssignedTasks(workers);
                UpdateReports(workers);
                return workers;
            }

            return null;
        }

        private static void UpdateReports(List<Worker> workers)
        {
            foreach (var worker in workers)
            {
                if (worker.Report != null)
                {
                    worker.Report.Tasks = JsonConvert.DeserializeObject<List<Task>>(worker.Report.TasksString,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                }
            }
        }

        private static void UpdateActionsAndAssignedTasks(List<Worker> workers)
        {
            foreach (var worker in workers)
            {
                if (worker.AssignedTasksStrings != null)
                {
                    worker.AssignedTasks = JsonConvert.DeserializeObject<List<Task>>(worker.AssignedTasksStrings,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                }

                if (worker.ActionsOnTasksString != null)
                {
                    worker.ActionsOnTasks = JsonConvert.DeserializeObject<List<Command>>(worker.ActionsOnTasksString,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        });
                }
            }
        }

        public static void ResetSubordinatesAfterSerialization(List<Worker> workers)
        {
            foreach (var worker in workers)
            {
                if (worker.Leader == null) continue;
                var leader = workers.Find(t => t.ID == worker.Leader.ID);
                if (leader != null)
                {
                    if (leader.Subordinates.Find(t => t.ID == worker.ID) == null)
                    {
                        leader.Subordinates.Add(worker);
                    }
                }
            }
        }
    }
}