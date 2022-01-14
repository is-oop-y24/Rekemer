namespace Banks.UI
{
    public abstract class AccountUI
    {
#pragma warning disable SA1401
        protected Account.Account _account;
#pragma warning restore SA1401

        protected AccountUI(Account.Account account)
        {
            _account = account;
        }

        public abstract void Menu();
    }
}