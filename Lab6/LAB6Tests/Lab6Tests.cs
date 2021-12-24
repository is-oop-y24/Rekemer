using System.Collections.Generic;
using NUnit.Framework;
using ServerApplication;
using ServerApplication.BusinessLayer;
using ServerApplication.Common.Models;
using ServerApplication.DataLayer.LogSystem;
using ServerApplication.DataLayer.Repository;
using ServerApplication.InterfaceLayer;

namespace LAB6Tests
{
    public class Tests
    {
       

        [Test]
        public void AddWorkers_HierarchyExists()
        {
            Manager manager = new Manager(new JsonDataContext());
            Worker worker = new Worker("ilia");
            Worker workerLeader = new Worker("boss",true);
            Worker worker2 = new Worker("maria");
            manager.WorkersController.AddWorker(workerLeader);
            manager.WorkersController.AddWorker(workerLeader,worker);
            manager.WorkersController.AddWorker(workerLeader,worker2);
            Assert.AreEqual(true,workerLeader.ID == worker.Leader.ID );
        }

        [Test]
        public void AddSubordinateToNotLeader_HierarchyIsCorrect()
        {
            Manager manager = new Manager(new JsonDataContext());
            Worker worker = new Worker("ilia");
            Worker worker2 = new Worker("maria");
            manager.WorkersController.AddWorker(worker,worker2);
            Assert.AreEqual(null, worker.Leader);
        }
        [Test]
        public void SerializeAndDeserialize_DataIsSaved()
        {
            var jsonContext = new JsonDataContext();
            Manager manager = new Manager(jsonContext);
            Worker worker = new Worker("ilia");
            Worker workerLeader = new Worker("boss", true);
            Worker worker2 = new Worker("maria");
            manager.WorkersController.AddWorker(workerLeader);
            manager.WorkersController.AddWorker(workerLeader, worker);
            manager.WorkersController.AddWorker(workerLeader, worker2);
            manager.Save();
            Manager manager2 = new Manager(jsonContext);
            List<Worker> workers =  manager2.WorkersController.GetWorkers();
            Assert.AreEqual(true, manager2.WorkersController.GetWorkerById(workerLeader.ID) == manager.WorkersController.GetWorkerById(workerLeader.ID));
        }

        [Test]
        public void SomethingWithLog()
        {
            ILog log = new FileLog(@"C:\lab5\log.txt");
            Log.Init(log);
            var jsonContext = new JsonDataContext();
            Manager manager = new Manager(jsonContext);
            Worker workerLeader = new Worker("boss", true);
            manager.WorkersController.AddWorker(workerLeader);
            var task = new Task();
            manager.TasksController.AddTask(task);
            manager.TasksController.AddCommentToTask(task,"this is task", workerLeader);
            string infoAboutAction = Log.Instance.FindTimesOfParticularAction("Comment");
            Assert.AreEqual(true, infoAboutAction != null);

        }

        [Test]
        public void AddCommands_CommandsAreSaved()
        {
            ILog log = new FileLog(@"C:\lab5\log.txt");
            Log.Init(log);
            var jsonContext = new JsonDataContext();
            Manager manager = new Manager(jsonContext);
            Worker workerLeader = new Worker("boss", true);
            manager.WorkersController.AddWorker(workerLeader);
            var task = new Task();
            manager.TasksController.AddTask(task);
            manager.TasksController.ChangeStateOfTask(task,TaskState.Active, workerLeader);
            manager.Save();
            manager.WorkersController.Delete(workerLeader);
            manager.Load();
            if ( manager.WorkersController.GetWorkers().Count == 1)
            {
                Assert.AreEqual(true, workerLeader.ID == manager.WorkersController.GetWorkers()[0].ID);
            }
           
        }
        
        
        // can get tasks of subordinates
        [Test]
        public void CreateSubordinated_GetTheirTasks()
        {
            ILog log = new FileLog(@"C:\lab5\log.txt");
            Log.Init(log);
            var jsonContext = new JsonDataContext();
            Manager manager = new Manager(jsonContext);
            Worker worker2 = new Worker("ilia");
            Worker workerLeader = new Worker("boss", true);
            Worker worker3 = new Worker("maria");
            manager.WorkersController.AddWorker(workerLeader,worker2);
            manager.WorkersController.AddWorker(workerLeader,worker3);
            Task task2 = new Task();
            Task task3 = new Task();
            manager.TasksController.AddTask(task2,worker2);
            manager.TasksController.AddTask(task3,worker3);
            var tasks =manager.TasksController.GetAllTasksOfSubordinates(workerLeader);
            Assert.AreEqual(2, tasks.Count);
        }

        [Test]
        public void AddTeamLead_HierarchyExists()
        {
            Worker lead= new TeamLead("john");
            lead.IsTeamLead();
            Manager manager = new Manager(new JsonDataContext());
            Worker worker = new Worker("ilia");
            worker.IsTeamLead();
            Worker workerLeader = new Worker("boss", true);
            Worker worker2 = new Worker("another ilia");
            manager.WorkersController.AddWorker(workerLeader);
            manager.WorkersController.AddWorker(lead);
            manager.WorkersController.AddWorker(workerLeader, worker);
            manager.WorkersController.AddWorker(workerLeader, worker2);
            manager.WorkersController.AddWorker(worker, worker2);
            manager.WorkersController.UpdateHierarchy();
            Assert.AreEqual(true,lead.Subordinates.Find( t=> t.ID == workerLeader.ID) != null);
        }
        //tests with report
        [Test]
        public void TryToSaveNotFinishedReport_ReportIsStillEdit()
        {
            ILog log = new FileLog(@"C:\lab5\log.txt");
            Log.Init(log);
            Manager manager = new Manager(new JsonDataContext());
            Worker worker = new Worker("ilia");
            Worker workerLeader = new Worker("boss", true);
            manager.WorkersController.AddWorker(workerLeader);
            manager.WorkersController.AddWorker(workerLeader, worker);
            
            var report = new Report("description");
            var task = new Task();
            manager.TasksController.AddCommentToTask(task,"comment",worker);
            manager.TasksController.AssignReport(worker,new Report("description"));
            manager.TasksController.AddTaskToReport(task,report);
            Assert.AreEqual(false,manager.TasksController.SaveReport(report)); 
        }

        [Test]
        public void ChangeAssignedWorker_AssignedWorkerIsChanged()
        {
            ILog log = new FileLog(@"C:\lab5\log.txt");
            Log.Init(log);
            Manager manager = new Manager(new JsonDataContext());
            Worker worker = new Worker("ilia");
            Worker workerLeader = new Worker("boss", true);
            manager.WorkersController.AddWorker(workerLeader);
            manager.WorkersController.AddWorker(workerLeader, worker);
            var task = new Task();
            manager.TasksController.AddTask(task);
            manager.TasksController.AddCommentToTask(task,"comment",worker);
            manager.TasksController.ChangeAssignedWorkerOfTask(task,workerLeader,worker);
            if (manager.TasksController.GetTaskById(task.ID).AssignedWorkers.Count == 1)
            {
                Assert.AreEqual(true, manager.TasksController.GetTaskById(task.ID).AssignedWorkers[0].ID == workerLeader.ID); 
            }
        }

        [Test]
        public void GetTasksAssignedToWorker_TasksGot()
        {
            ILog log = new FileLog(@"C:\lab5\log.txt");
            Log.Init(log);
            Manager manager = new Manager(new JsonDataContext());
            Worker worker = new Worker("ilia");
            Worker workerLeader = new Worker("boss", true);
            manager.WorkersController.AddWorker(workerLeader);
            manager.WorkersController.AddWorker(workerLeader, worker);
            var task = new Task();
            var task2 = new Task();
            manager.TasksController.AddTask(task,worker);
            manager.TasksController.AddTask(task2,worker);
            if (worker.AssignedTasks.Count == 2)
            {
                Assert.AreEqual(false, worker.AssignedTasks.Find(t => t.ID == task.ID ) == null); 
            }
        }
    }
}