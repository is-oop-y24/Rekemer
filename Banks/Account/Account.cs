using System;
using System.Collections.Generic;

namespace Banks.Account
{
    public abstract class Account
    {
        public CommandProccesor CommandProccesor { get; } = new CommandProccesor();
        protected List<Command.Command> CommandsToExecuteInTheEndOfMonth = new List<Command.Command>();
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public readonly Bank Bank;
        protected bool _isBonusMoney = true;
        protected decimal _money;
        public abstract AccountUI UI { get; }
        public readonly decimal Limit;
        public readonly Client Client;
        public abstract float Percent { get; }

        public readonly Guid Id = Guid.NewGuid();

        public List<Command.Command> HistoryCommands
        {
            get => CommandProccesor.Commands;
        }

        public abstract decimal Money { get; }


        public string NameOfBank
        {
            get => Bank.Name;
        }


        protected Account(DateTime endTime, Bank bank, decimal initialMoney, Client client)
        {
            StartTime = DateTime.Now;
            EndTime = endTime;
            Bank = bank;
            Client = client;
            Client.AddAccount(this);
            _money = initialMoney;
            Limit = bank.Limit;
        }

        public bool IsValid()
        {
            if (Time.Instance.CurrentTime >= EndTime)
            {
                return false;
            }

            return true;
        }

        public abstract void AddMoney(decimal money);
        public abstract bool CanWithdraw(decimal money);
        public abstract void Update();
        public abstract bool Update(decimal money);
        protected abstract decimal CalculateMoney();

        public void UndoCommand(Command.Command command)
        {
            CommandProccesor.Undo(command);
        }

        public void UndoAll()
        {
            CommandProccesor.Undo();
        }

        public void TestFunc(Command.Command command)
        {
            CommandProccesor.ExecuteCommand(command);
        }
    }
}