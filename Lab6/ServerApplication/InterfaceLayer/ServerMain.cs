using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerApplication.Common.Models;
using ServerApplication.DataLayer.DataContext;

namespace ServerApplication.InterfaceLayer
{
    class Program
    {
        static Manager _manager = new Manager(new JsonDataContext());

        static void Main(string[] args)
        {
            _manager.Load();


            Socket m_ListenSockeet;
            m_ListenSockeet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind the socket
            int iPort = 2085; // Set port number
            IPEndPoint m_LocalIPEndPoint = new IPEndPoint(IPAddress.Any, iPort);
            m_ListenSockeet.Bind(m_LocalIPEndPoint); // Bind

            // Optional code to display IP Address and Port Number
            Console.WriteLine(" Server IP Address : " + LocalIPAddress());
            Console.WriteLine(" Listening on Port : " + iPort);

            // Start Listening
            m_ListenSockeet.Listen(4);

            Socket m_sendSocket;
            m_sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress destinationIP = IPAddress.Parse(LocalIPAddress());
            IPEndPoint destinationEP = new IPEndPoint(destinationIP, 2084);
            // User Message
            Console.WriteLine("\nWaiting to Connect... ");


            m_sendSocket.Connect(destinationEP);

            // User Message
            Console.WriteLine("Connected... ");
            m_sendSocket.Send(Encoding.ASCII.GetBytes("from server"), SocketFlags.None);
            // Accept incoming connection
            Socket m_AcceptedSocket = m_ListenSockeet.Accept();


            byte[] ReceiveBuffer = new byte[1024];
            int iReceiveByteCount;
            string msg = "";

            while (msg != "quit")
            {
                //Display the message
                iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);
                msg = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                Console.WriteLine(msg);
                var commands = ParseRequest(msg);
                byte[] b_Data = System.Text.Encoding.ASCII.GetBytes(ProccessRequest(commands));
                Console.WriteLine(Encoding.Default.GetString(b_Data));
                m_sendSocket.Send(b_Data, SocketFlags.None);
            }

            m_AcceptedSocket.Shutdown(SocketShutdown.Both);


            m_AcceptedSocket.Close();
        }

        private static string ProccessRequest(string[] commands)
        {
            string response = "";
            if (commands[0] != null && commands[0] == "GetWorkers")
            {
                var workers = _manager.WorkersController.GetWorkers();
                if (workers != null)
                {
                    foreach (var worker in workers)
                    {
                        var id = worker.ID;
                        var name = worker.Name;
                        response += "\n" + "Name" + name + "\n" + "ID" + id + "\n";
                    }

                    return response;
                }

                return "There are no workers";
            }

            if (commands[0] != null && commands[0] == "GetAllTaskOfWorker" && commands[1] != null)
            {
                var tasks = _manager.WorkersController.GetAllTaskOfWorker(commands[1]);
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        response += GetDataOfTask(task);
                    }

                    return response;
                }

                return "There are no tasks";
            }

            if (commands[0] != null && commands[0] == "FindWorkersWithDoneReports")
            {
                var workers = _manager.WorkersController.FindWorkersWithDoneReports();
                if (workers != null)
                {
                    foreach (var worker in workers)
                    {
                        var id = worker.ID;
                        var name = worker.Name;
                        response += "\n" + "Name" + name + "\n" + "ID" + id + "\n";
                    }

                    return response;
                }

                return "There are no workers with done reports";
            }

            if (commands[0] != null && commands[0] == "FindWorkersWithNotDoneReports")
            {
                var workers = _manager.WorkersController.FindWorkersWithNotDoneReports();
                if (workers != null)
                {
                    foreach (var worker in workers)
                    {
                        var id = worker.ID;
                        var name = worker.Name;
                        response += "\n" + "Name" + name + "\n" + "ID" + id + "\n";
                    }

                    return response;
                }

                return "All reports are done";
            }

            if (commands[0] != null && commands[0] == "Delete" && commands[1] != null)
            {
                if (_manager.WorkersController.Delete(commands[1]))
                {
                    _manager.TasksController.DeleteWorkerFromTasks(commands[1]);
                    _manager.Save();
                    return $"workers with id {commands[1]} deleted";
                }

                return "there is no such worker";
            }

            if (commands[0] != null && commands[0] == "Create" && commands[1] != null)
            {
                if (commands[2] != null)
                {
                    _manager.WorkersController.Create(commands[1], string.Equals(commands[2], "true"));
                    _manager.Save();
                    return "success";
                }
                else
                {
                    _manager.WorkersController.Create(commands[1]);
                    return "success";
                }
            }

            if (commands[0] != null && commands[0] == "GetAllReports")
            {
                var reports = _manager.WorkersController.GetAllReports();
                if (reports != null)
                {
                    foreach (var report in reports)
                    {
                        var description = report.Description;
                        var status = report.ReportStatus;
                        var id = report.Id;
                        response += "\n" + "id" + id + "\n" + "status" + status + "\n" + "description" + description +
                                    "\n";
                    }

                    return response;
                }

                return "there are no reports";
            }

            if (commands[0] != null && commands[0] == "AddTask" && commands[1] != null && commands[2] != null)
            {
                var task = new ServerApplication.Common.Models.Task();
                task.Comment = commands[1];
                _manager.TasksController.AddTask(task, _manager.WorkersController.GetWorkerById(commands[2]));
                _manager.Save();
            }

            if (commands[0] != null && commands[0] == "GetTasksWithLatestChanges")
            {
                var tasks = _manager.TasksController.GetTasksWithLatestChanges();
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        response += GetDataOfTask(task);
                    }

                    return response;
                }

                return "there are no tasks";
            }

            if (commands[0] != null && commands[0] == "GetTaskWithThisDateTime" && commands[1] != null)
            {
                var tasks = _manager.TasksController.GetTaskWithThisDateTime(commands[1]);
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        response += GetDataOfTask(task);
                    }

                    return response;
                }

                return "check data or there is no such task";
            }

            if (commands[0] != null && commands[0] == "GetTaskById" && commands[1] != null)
            {
                var task = _manager.TasksController.GetTaskById(commands[1]);
                if (task != null)
                {
                    response = GetDataOfTask(task);
                    return response;
                }

                return "there is no such task";
            }

            if (commands[0] != null && commands[0] == "GetTasksChangedByWorker" && commands[1] != null)
            {
                var tasks = _manager.TasksController.GetTasksChangedByWorker(
                    _manager.WorkersController.GetWorkerById(commands[1]));
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        response += GetDataOfTask(task);
                    }

                    return response;
                }

                return "there are no tasks";
            }

            if (commands[0] != null && commands[0] == "GetReportsOfSubordinates" && commands[1] != null)
            {
                var reports =
                    _manager.TasksController.GetReportsOfSubordinates(
                        _manager.WorkersController.GetWorkerById(commands[1]));
                if (reports != null)
                {
                    foreach (var report in reports)
                    {
                        var description = report.Description;
                        var status = report.ReportStatus;
                        var id = report.Id;
                        response += "\n" + "id" + id + "\n" + "status" + status + "\n" + "description" + description +
                                    "\n";
                    }

                    return response;
                }

                return "there are no reports";
            }

            if (commands[0] != null && commands[0] == "AddTaskToReport" && commands[1] != null && commands[2] != null)
            {
                var reportToAddTaskTo = _manager.WorkersController.GetAllReports().Find(t => t.Id == commands[2]);
                if (reportToAddTaskTo != null)
                {
                    var task = new ServerApplication.Common.Models.Task();
                    task.Comment = commands[1];
                    _manager.TasksController.AddTaskToReport(task, reportToAddTaskTo);
                    _manager.Save();
                    return "success";
                }

                return "there is no report with such id";
            }

            if (commands[0] != null && commands[0] == "UpdateReportDescription" && commands[1] != null &&
                commands[2] != null)
            {
                _manager.TasksController.UpdateReportDescriprion(
                    _manager.WorkersController.GetAllReports().Find(t => t.Id == commands[1]), commands[2]);
                _manager.Save();
                return "success";
            }

            if (commands[0] != null && commands[0] == "SaveReport" && commands[1] != null)
            {
                if (_manager.TasksController.SaveReport(_manager.WorkersController.GetAllReports()
                    .Find(t => t.Id == commands[1])))
                {
                    _manager.Save();
                    return "success";
                }

                return "report is not saved, not all tasks are completed";
            }

            if (commands[0] != null && commands[0] == "GetAllTasksOfSubordinates" && commands[1] != null)
            {
                var tasks = _manager.TasksController.GetAllTasksOfSubordinates(
                    _manager.WorkersController.GetWorkerById(commands[1]));
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        response += GetDataOfTask(task);
                    }

                    return response;
                }

                return $"there are no tasks of subordinates of worker {commands[1]}";
            }

            if (commands[0] != null && commands[0] == "AddCommentToTask" && commands[1] != null &&
                commands[2] != null && commands[3] != null)
            {
                var task = _manager.TasksController.GetTaskById(commands[1]);
                if (task == null) return "there is no such task";
                var worker = _manager.WorkersController.GetWorkerById(commands[3]);
                if (worker == null) return "there is no such worker";
                _manager.TasksController.AddCommentToTask(task, commands[2], worker);
                _manager.Save();
                return "success";
            }

            if (commands[0] != null && commands[0] == "ChangeStateOfTask" && commands[1] != null &&
                commands[2] != null && commands[3] != null)
            {
                var task = _manager.TasksController.GetTaskById(commands[1]);
                if (task == null) return "there is no such task";
                var worker = _manager.WorkersController.GetWorkerById(commands[3]);
                if (worker == null) return "there is no such worker";
                var state = Enum.TryParse(commands[2], out TaskState myStatus);
                if (!state) return "there is no such type of task";
                _manager.TasksController.ChangeStateOfTask(task, myStatus, worker);
                _manager.Save();
                return "success";
            }

            if (commands[0] != null && commands[0] == "ChangeAssignedWorkerOfTask" && commands[1] != null &&
                commands[2] != null && commands[3] != null)
            {
                var newWorker = _manager.WorkersController.GetWorkerById(commands[2]);
                if (newWorker == null) return $"there is no such worker with id {commands[2]}";
                var oldWorker = _manager.WorkersController.GetWorkerById(commands[3]);
                if (oldWorker == null) return $"there is no such worker with id {commands[3]}";
                var task = _manager.TasksController.GetTaskById(commands[1]);
                if (task == null) return "there is no such task";
                _manager.TasksController.ChangeAssignedWorkerOfTask(task, newWorker, oldWorker);
                _manager.Save();
                return "success";
            }

            if (commands[0] != null && commands[0] == "AssignTaskToWorker" && commands[1] != null &&
                commands[2] != null)
            {
                var task = _manager.TasksController.GetTaskById(commands[1]);
                if (task == null) return "there is no such task";
                var worker = _manager.WorkersController.GetWorkerById(commands[2]);
                if (worker == null) return "there is no such worker";
                _manager.TasksController.AssignTaskToWorker(task, worker);
                _manager.Save();
                return "success";
            }

            if (commands[0] != null && commands[0] == "AssignReport" && commands[1] != null &&
                commands[2] != null)
            {
                var report = _manager.WorkersController.GetAllReports().Find(t => t.Id == commands[1]);
                if (report == null) return "there is no such report";
                var worker = _manager.WorkersController.GetWorkerById(commands[2]);
                if (worker == null) return "there is no such worker";
                _manager.TasksController.AssignReport(worker, report);
                _manager.Save();
            }

            return "request is not parsed";
        }

        private static string GetDataOfTask(ServerApplication.Common.Models.Task task)
        {
            var id = task.ID;
            var name = task.Comment;
            var timeOfChange = task.LastTimeChanged;
            var dateCreation = task.TimeOfBirth;
            return "\n" + "Name" + name + "\n" + "ID" + id + "\n" + "LastTimeOfChange" + timeOfChange +
                   "\n" + "DateOfCreation" + dateCreation + "\n";
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName()); //gets the name of the current host
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) //Finds the IP which maches internetwork
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            return localIP;
        }

        public static string[] ParseRequest(string request)
        {
            string[] commands = request.Split(' ');
            return commands;
        }
    }
}