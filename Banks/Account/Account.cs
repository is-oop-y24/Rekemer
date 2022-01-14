using System;
using System.Collections.Generic;
using Banks.UI;

namespace Banks.Account
{
    public abstract class Account
    {
#pragma warning disable SA1306
#pragma warning disable SA1401
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public readonly decimal Limit;
        public readonly Client Client;
        public readonly Bank Bank;
        public readonly Guid Id = Guid.NewGuid();
        protected List<Command.Command> CommandsToExecuteInTheEndOfMonth = new List<Command.Command>();
        protected bool _isBonusMoney = true;
        protected decimal _money;
        protected Account(
            DateTime endTime,
            Bank bank,
            decimal initialMoney,
            Client client)
        {
            StartTime = DateTime.Now;
            EndTime = endTime;
            Bank = bank;
            Client = client;
            Client.AddAccount(this);
            _money = initialMoney;
            Limit = bank.Limit;
        }

        public abstract float Percent { get; }
        public CommandProccesor CommandProccesor { get; } = new CommandProccesor();
#pragma warning restore SA1401
#pragma warning restore SA1306

        public List<Command.Command> HistoryCommands
        {
            get => CommandProccesor.Commands;
        }

        public abstract decimal Money { get; }

        public string NameOfBank
        {
            get => Bank.Name;
        }

        public bool IsValid()
        {
            if (Time.Instance.CurrentTime >= EndTime)
            {
                return false;
            }

            return true;
        }

        public abstract AccountUI DeriveUI();

        public abstract void AddMoney(decimal money);
        public abstract bool CanWithdraw(decimal money);
        public abstract void Update();
        public abstract bool Update(decimal money);
        public void UndoCommand(Command.Command command)
        {
            CommandProccesor.Undo(command);
        }

        public void UndoAll()
        {
            CommandProccesor.Undo();
        }

        // for tests
        public void TestFunc(Command.Command command)
        {
            CommandProccesor.ExecuteCommand(command);
        }

        protected abstract decimal CalculateMoney();
    }
}