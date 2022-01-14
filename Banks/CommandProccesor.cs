using System.Collections.Generic;

namespace Banks
{
    public class CommandProccesor
    {
#pragma warning disable SA1401
        public List<Command.Command> Commands = new List<Command.Command>();
#pragma warning restore SA1401
        private int _currentIndex;

        public void ExecuteCommand(Command.Command command)
        {
            Commands.Add(command);
            command.Execute();
            _currentIndex = Commands.Count - 1;
        }

        public void Undo(Command.Command command)
        {
            if (Commands.Contains(command))
            {
                command.Undo();
            }
        }

        public void Undo()
        {
            if (_currentIndex < 0) return;
            if (Commands.Count == 0) return;
            while (_currentIndex >= 0)
            {
                Commands[_currentIndex].Undo();
                _currentIndex--;
            }

            Commands.Clear();
        }
    }
}