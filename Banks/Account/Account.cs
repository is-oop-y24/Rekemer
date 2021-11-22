using System;
using System.Collections.Generic;

namespace Banks.Account
{
    public abstract class Account
    {
        protected CommandProccesor CommandProccesor { get; } = new CommandProccesor();
        protected List<Command.Command> CommandsToExecuteInTheEndOfMonth = new List<Command.Command>();
        protected DateTime StartTime;
        protected DateTime EndTime;
        protected Bank _bank;
        protected bool _isBonusMoney = true;
        protected double _money;

        public readonly double Limit;
        public readonly Client Client;
        protected abstract float _percent { get; set; }

        public float Percent
        {
            get => _percent;
        }

        public List<Command.Command> HistoryCommands
        {
            get => CommandProccesor.Commands;
        }

        public abstract double Money { get; }


        public string nameOfBank
        {
            get => _bank.Name;
        }


        protected Account(DateTime endTime, Bank bank, double initialMoney, Client client)
        {
            StartTime = DateTime.Now;
            EndTime = endTime;
            _bank = bank;
            Client = client;
            Client.AddAccount(this);
            _money = initialMoney;
            Limit = bank.Limit;
        }

        public abstract void AddMoney(double money);
        public abstract bool CanWithdraw(double money);
        public abstract void Update();
        public abstract bool Update(double money);
        protected abstract double CalculateMoneyWithProcents(int days);
        public abstract void Menu();

        public abstract void UpdatePercent(float percent);

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