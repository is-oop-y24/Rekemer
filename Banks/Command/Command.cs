namespace Banks.Command
{
    public abstract class Command
    {
        protected Account.Account _account;

        public Command(Account.Account account)
        {
            _account = account;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}