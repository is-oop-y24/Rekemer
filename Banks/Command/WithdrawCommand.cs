namespace Banks.Command
{
    public class WithdrawCommand : Command
    {
        private readonly decimal _money;
        private readonly Client _client;
        private bool _isUndo;
        public WithdrawCommand(Account.Account account, decimal money)
            : base(account)
        {
            _money = money;
            _client = account.Client;
        }

        public override void Execute()
        {
            if (_account.CanWithdraw(_money))
            {
                _account.AddMoney(-_money);
                _client.AddMoney(_money);
            }
        }

        public override void Undo()
        {
            if (_isUndo == false)
            {
                 _account.AddMoney(_money);
                 _client.Substract(_money);
                 _isUndo = true;
            }
        }
    }
}