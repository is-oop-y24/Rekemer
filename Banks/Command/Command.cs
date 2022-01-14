namespace Banks.Command
{
    public abstract class Command
    {
#pragma warning disable SA1401
        protected Account.Account _account;
#pragma warning restore SA1401

        public Command(Account.Account account)
        {
            _account = account;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}