using Newtonsoft.Json;
using ServerApplication.Common.Models;

namespace ServerApplication.Common.Commands
{
    public class AddCommentCommand : Command
    {
        [JsonProperty("Comment")] private string _comment;

        public AddCommentCommand(Task task, string comment) : base(task)
        {
            _comment = comment;
        }

        public override void Execute()
        {
            _task.Comment += "\n " + _comment;
        }

        public override string NameOfAction()
        {
            return "AddComment";
        }
    }
}