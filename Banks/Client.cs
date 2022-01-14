using System;
using System.Collections.Generic;

namespace Banks
{
    public class Client
    {
    #pragma warning disable SA1401
        public Guid Id = Guid.NewGuid();
    #pragma warning restore SA1401
        private string _address = string.Empty;
        private string _passport = string.Empty;
        private ClientStatus _status = ClientStatus.Dubious;
        private List<Account.Account> _accounts = new List<Account.Account>();

        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public decimal PocketMoney { get; private set; }

        public List<Account.Account> Accounts
        {
            get => _accounts;
        }

        public ClientStatus Status
        {
            get => _status;
            private set => _status = value;
        }

        public string Name { get; set; }
        public string Surname { get; set; }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string Passport
        {
            get => _passport;
            set => _passport = value;
        }

        public void SetPassport(string passport)
        {
            Passport = passport;
            if (Address != string.Empty)
            {
                Status = ClientStatus.Full;
            }
        }

        public void Update(string notification)
        {
            // do something with this notification - show on his phone, client's accounts are  already updated  at this point
        }

        public void SetAddress(string address)
        {
            this.Address = address;
            if (Passport != string.Empty)
            {
                Status = ClientStatus.Full;
            }
        }

        public void AddMoney(decimal money)
        {
            PocketMoney += money;
        }

        public void AddAccount(Account.Account account)
        {
            _accounts.Add(account);
        }

        public void Substract(decimal money)
        {
            PocketMoney -= money;
        }
    }
}