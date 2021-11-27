using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks
{
    public class CentralBank
    {
        private static CentralBank _instance;
        public List<Bank> Banks { get; private set; } = new List<Bank>();

        public static CentralBank Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CentralBank();
                }

                return _instance;
            }
        }

        public void AddBanks(List<Bank> banks)
        {
            if (banks == null) return;
            Banks = Banks.Union(banks).ToList();
        }


        public void Notify()
        {
            var banks = CentralBank.Instance.Banks;
            if (banks == null) return;
            foreach (var bank in banks)
            {
                bank.Update();
            }
        }

        public Account.Account FindAccount(string id)
        {
            // get all accounts and find the right one by id
            List<Account.Account> allAccounts = new List<Account.Account>();
            foreach (var bank in Banks)
            {
                var bankAccounts = bank.GetAllAccounts();
                if (bankAccounts.Count > 0)
                {
                    allAccounts = allAccounts.Union(bankAccounts).ToList();
                }
            }

            foreach (var account in allAccounts)
            {
                if (account.Id.ToString() == id)
                {
                    return account;
                }
            }
            return null;
        }
    }
}