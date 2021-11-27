namespace Banks
{
    public abstract class AccountUI
    {
        protected Account.Account _account;

        protected AccountUI(Account.Account account)
        {
            _account = account;
        }

        public abstract void Menu();
    }
}