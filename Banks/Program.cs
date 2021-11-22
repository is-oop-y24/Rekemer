using System;
using System.Collections.Generic;
using Banks.Account;

namespace Banks
{
    internal static class Program
    {
      
        private static void Main()
        {
            Bank bank = new Bank("Bank");
            Client client = new Client("Ilia", "Zhidelev");
            client.AddMoney(100d);
            bank.CnahgeDebAccSetitings(3.65f);
            List<double> moneyThresholds = new List<double>() {10, 20, 40}; 
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f , 2.7f, 4f};
            bank.CnahgeDepAccSetitings(moneyThresholds,percentThresholds);
            bank.CnahgeCredAccSetitings(200,2);
            bank.CnahgeLimitForShadyAccountsSettings(100d);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            Account.Account debitAccount = new DebitAccount(DateTime.Today.AddYears(2), bank, 100000d,  client);
            Account.Account creditAccount = new CreditAccount(DateTime.Today.AddYears(1), bank, 100000d, bank.Commission ,client);
            bank.Register(client, debitAccount);
            bank.Register(client, creditAccount);
            Time.Instance.ResetTime();
            Time.Instance.AddTime(0,0,31);
            Time.Instance.AddTime(0,0,31);
            ConsoleUI.Instance.MainMenu();
        }
    }

}