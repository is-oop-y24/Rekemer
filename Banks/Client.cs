using System;
using System.Collections.Generic;

namespace Banks
{
    public class Client
    {
        private string _address = String.Empty;
        private string _passport = String.Empty;
        private ClientStatus _status = ClientStatus.Dubious;
        private List<Account.Account> _accounts = new List<Account.Account>();
        public decimal PocketMoney { get; private set; }
        public readonly ClientUI UI;
        public Guid id = Guid.NewGuid();
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

        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
            UI = new ClientUI(this);
        }

        public void SetPassport(string passport)
        {
            Passport = passport;
            if (Address != String.Empty)
            {
                Status = ClientStatus.Full;
            }
        }

        public void Update(string notification)
        {
            // do something with this notification - show on his phone, client's accounts are  already updated  at this point
        }

        public void SetAddress(string Address)
        {
            this.Address = Address;
            if (Passport != String.Empty)
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