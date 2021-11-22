using System;

namespace Banks
{
    public class TransferCommand : Command.Command
    {
        private Account.Account _accountTo;
        private readonly double _money;

        public TransferCommand(Account.Account account, Account.Account accountTo,  double money) : base(account)
        {
            _accountTo = accountTo;
            _money = money;
        }

        public override void Execute()
        {
           _accountTo.AddMoney(_money);
           _account.AddMoney(-_money);
            
        }

        public override void Undo()
        {
            _accountTo.AddMoney(-_money);
            _account.AddMoney(_money);
        }
    }
}