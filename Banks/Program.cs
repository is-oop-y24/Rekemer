using System;
using System.Collections.Generic;
using Banks.Account;
using Banks.UI;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            Bank bank = new Bank("Bank");
            Client client = new Client("Ilia", "Zhidelev");
            client.AddMoney(100);
            bank.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() { 10, 20, 40 };
            List<float> percentThresholds = new List<float>() { 2.2f, 2.6f, 2.7f, 4f };
            bank.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            bank.ChangeCredAccSettings(200, 2);
            bank.ChangeLimitForShadyAccountsSettings(100);
            CentralBank.Instance.AddBanks(new List<Bank>() { bank });
            Account.Account debitAccount = new DebitAccount(DateTime.Today.AddYears(2), bank, 100000,  client);
            Account.Account creditAccount = new CreditAccount(DateTime.Today.AddYears(1), bank, 100000, client);
            bank.Register(client, debitAccount);
            bank.Register(client, creditAccount);
            Time.Instance.ResetTime();
            Time.Instance.AddTime(0, 0, 31);
            Time.Instance.AddTime(0, 0, 31);
            if (UIManager.Instance != null)
            {
                UIManager.Instance.MainMenu();
            }
        }
    }
}