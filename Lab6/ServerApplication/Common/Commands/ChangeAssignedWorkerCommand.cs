using Newtonsoft.Json;
using ServerApplication.Common.Models;

namespace ServerApplication.Common.Commands
{
    public class ChangeAssignedWorkerCommand : Command
    {
        [JsonProperty("newWorker")] private Worker _newWorker;
        [JsonProperty("oldWorker")] private Worker _oldWorker;

        public ChangeAssignedWorkerCommand(Task task, Worker newWorker, Worker oldWorker = null) : base(task)
        {
            _newWorker = newWorker;
            _oldWorker = oldWorker;
        }

        public override void Execute()
        {
            if (_oldWorker != null)
            {
                _task.AssignedWorkers.Remove(_oldWorker);
            }

            _task.AssignedWorkers.Add(_newWorker);
            _newWorker.AssignedTasks.Add(_task);
        }

        public override string NameOfAction()
        {
            return "ChangeAssignedWorker";
        }
    }
}