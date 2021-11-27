using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Command;

namespace Banks
{
    public class DepositAccount : Account.Account
    {
        private DateTime _lastUpdate;
        private bool canGetOutside;

        public DepositAccount(DateTime endTime, Bank bank, decimal money, Client client) : base(endTime,
            bank, money, client)
        {
            _UI = new DepositAccountUI(this);
        }

        public override decimal Money
        {
            get
            {
                if (canGetOutside)
                {
                    return _money;
                }

                return 0;
            }
        }

        public override AccountUI UI { get => _UI; }
        private DepositAccountUI _UI;
        public override float Percent
        {
            get => Bank.DeterminePercent(_money);
        }

        public override void AddMoney(decimal money)
        {
            _money += money;
        }

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
            if (Time.Instance.CurrentTime >= EndTime)
            {
                if (money < Money)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Update()
        {
            if (!IsValid())
            {
                canGetOutside = true;
                return;
            }
            

            // find out how many days are passed
            var passedDays = (Time.Instance.CurrentTime - StartTime).Days;
            if (Math.Abs(Time.Instance.CurrentTime.Day - _lastUpdate.Day) == 1) _isBonusMoney = true;
            // consider all passed days
            for (int i = 0; i < passedDays; i++)
            {
                Command.Command command = new ReplenishCommand(this,  CalculateMoney());
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
            decimal increase = money / 100 * (decimal)Percent / 365;
            Command.Command command = new ReplenishCommand(this,  increase);
            CommandsToExecuteInTheEndOfMonth.Add(command);
            return true;
        }


        protected override decimal CalculateMoney()
        {
            return _money * (decimal)Bank.PercentDebAccount / (decimal)(100f * 365f);
        }

      

        
    }
}