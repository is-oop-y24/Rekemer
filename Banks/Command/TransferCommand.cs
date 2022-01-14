using System;

namespace Banks
{
    public class TransferCommand : Command.Command
    {
        private readonly decimal _money;
        private Account.Account _accountTo;

        public TransferCommand(Account.Account account, Account.Account accountTo,  decimal money)
            : base(account)
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