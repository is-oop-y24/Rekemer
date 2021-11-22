using System;

namespace Banks
{
    public class ReplenishCommand: Command.Command
    {
        private readonly double _money;
        private bool _isUndo;
        public ReplenishCommand(Account.Account account,   double money) : base(account)
        {
            _money = money;
        }

        public override void Execute()
        {
            _account.AddMoney(_money);
        }

        public override void Undo()
        {
            if (_isUndo == false)
            {
                _account.AddMoney(-_money);
                _isUndo = true;
            }
            
        }
    }
}