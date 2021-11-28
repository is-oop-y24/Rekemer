using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Command;
using Banks.UI;

namespace Banks.Account
{
    public class DebitAccount : Account
    {
        public override decimal Money
        {
            get => _money;
        }

       

        public override float Percent
        {
            get => Bank.PercentDebAccount;
        }

        public override AccountUI DeriveUI()
        {
            return new DebitAccountUI(this);
        }

        public override void AddMoney(decimal money)
        {
            _money += money;
        }

        private DateTime _lastUpdate = default;

        public override bool CanWithdraw(decimal money)
        {
            if (Client.Status == ClientStatus.Dubious)
            {
                if (Bank.Limit >= money && Money >= money)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (Money >= money)
                {
                    return true;
                }
            }

            return false;
        }


        // in the end of every month need to withdraw money
        public override void Update()
        {
            if (!IsValid())
            {
                return;
            }


            // find out how many days are passed
            var passedDays = (Time.Instance.CurrentTime - (_lastUpdate == default ? StartTime : _lastUpdate)).Days;
            if (Math.Abs(Time.Instance.CurrentTime.Day - _lastUpdate.Day) == 1) _isBonusMoney = true;
            // consider all passed days
            for (int i = 0; i < passedDays; i++)
            {
                Command.Command command = new ReplenishCommand(this, CalculateMoney());
                CommandsToExecuteInTheEndOfMonth.Add(command);
            }

            List<Command.Command> commandsToDelete = new List<Command.Command>();
            foreach (var comm in CommandsToExecuteInTheEndOfMonth)
            {
                if (passedDays >= 31)
                {
                    CommandProccesor.ExecuteCommand(comm);
                    commandsToDelete.Add(comm);
                }
            }

            CommandsToExecuteInTheEndOfMonth = CommandsToExecuteInTheEndOfMonth.Except(commandsToDelete).ToList();
            _lastUpdate = Time.Instance.CurrentTime;
        }

        public override bool Update(decimal money)
        {
            decimal increase = 0;

            if (_isBonusMoney)
            {
                increase = money / 100 * (decimal) Bank.PercentDebAccount / 365;
                _isBonusMoney = false;
            }

            Command.Command command = new ReplenishCommand(this, increase + money);
            CommandsToExecuteInTheEndOfMonth.Add(command);
            return true;
        }


        public DebitAccount(DateTime endTime, Bank bank, decimal money, Client client) : base(endTime,
            bank, money,
            client)
        {
           
        }

        protected override decimal CalculateMoney()
        {
            return _money * (decimal) Bank.PercentDebAccount / (decimal) (100f * 365f);
        }
    }
}