using Newtonsoft.Json;
using ServerApplication.Common.Models;

namespace ServerApplication.Common.Commands
{
    public class ChangeStateOfTask : Command
    {
        [JsonProperty("newState")] private TaskState _newState;

        public ChangeStateOfTask(Task task, TaskState newState) : base(task)
        {
            _newState = newState;
        }

        public override void Execute()
        {
            _task.State = _newState;
        }

        public override string NameOfAction()
        {
            return "ChangeStateOfTask";
        }
    }
}