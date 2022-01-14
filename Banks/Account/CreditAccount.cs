using System;
using System.Collections.Generic;
using Banks.Command;
using Banks.UI;

namespace Banks.Account
{
    public class CreditAccount : Account
    {
        public CreditAccount(DateTime endTime, Bank bank, decimal initialMoney, Client client)
            : base(
                endTime,
                bank,
                initialMoney,
                client)
        {
        }

        public override decimal Money
        {
            get => _money;
        }

        public override float Percent
        {
            get => 0f;
        }

        public override AccountUI DeriveUI()
        {
            return new CreditAccountUI(this);
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

            if (money < Money + Limit)
            {
                return true;
            }

            return false;
        }

        public override void Update()
        {
            if (!IsValid())
            {
                return;
            }
        }

        public override bool Update(decimal money)
        {
            Command.Command command = new ReplenishCommand(this, money);
            CommandsToExecuteInTheEndOfMonth.Add(command);
            return true;
        }

        protected override decimal CalculateMoney()
        {
            return 0;
        }
    }
}