using Newtonsoft.Json;
using ServerApplication.Common.Models;

namespace ServerApplication.Common.Commands
{
    public abstract class Command
    {
        protected Task _task;

        [JsonProperty("objectOfCommand")]
        public Task Task
        {
            get => _task;
        }

        protected Command(Task task)
        {
            _task = task;
        }

        public abstract void Execute();

        public abstract string NameOfAction();
    }
}