using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Account;
using Banks.Command;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTests
    {
        [Test]
        public void TimeHasPassed_MoneyIncreased()
        {
            Bank bank = new Bank("ilia Bank");
            Client client = new Client("someone", "who");
            bank.ChangeDebAccSettings(3.65f);
            Account.Account account = new DebitAccount(DateTime.Today.AddYears(4), bank, 100000, client);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            bank.Register(client, account);
            Time.Instance.ResetTime();
            Time.Instance.AddTime(0, 0, 31); // +10 to remember every day
            Assert.AreEqual(100310, (int) account.Money);
        }

        [Test]
        public void RegisterClientsWithAccount_ClientIsSaved()
        {
            Bank bank = new Bank("Ilia's Bank");
            Client client = new Client("someone", "who");
            Account.Account account = new DebitAccount(DateTime.Today.AddDays(2), bank, 4, client);
            bank.Register(client, account);
            Assert.AreEqual(client, bank.IsUserExist(client.Name, client.Surname));
        }

        [Test]
        public void RegisterClientsWithAccount_AccountIsSaved()
        {
            Bank bank = new Bank("Ilia's Bank");
            Client client = new Client("someone", "who");
            Account.Account account = new DebitAccount(DateTime.Today.AddDays(2), bank, 4, client);
            bank.Register(client, account);
            List<Account.Account> accounts = bank.GetAccountsOfUser(client);
            Assert.AreEqual(account, accounts[0]);
        }

        [Test]
        public void RegisterClientOnlyWihNameAndSurname_StatusIsDubious()
        {
            Bank bank = new Bank("Ilia's Bank");
            Client client = new Client("someone", "who");
            Account.Account account = new DebitAccount(DateTime.Today.AddDays(2), bank, 4, client);
            bank.Register(client, account);
            Assert.AreEqual(ClientStatus.Dubious, bank.IsUserExist(client.Name, client.Surname).Status);
        }

        [Test]
        public void RegisterClientFully_StatusIsFull()
        {
            Bank bank = new Bank("Ilia's Bank");
            Client client = new Client("someone", "who");
            Account.Account account = new DebitAccount(DateTime.Today.AddDays(2), bank, 4, client);
            bank.Register(client, account);
            client.SetAddress("ulitsa");
            client.SetPassport("2323113");
            Assert.AreEqual(ClientStatus.Full, bank.IsUserExist(client.Name, client.Surname).Status);
        }

        [Test]
        public void RegisterClientInFewBanks_ClientIsAwareOfBoth()
        {
            Bank bank = new Bank("Ilia's Bank");
            Bank bank2 = new Bank("bad Bank");
            Client client = new Client("someone", "who");
            Account.Account account = new DebitAccount(DateTime.Today.AddDays(2), bank, 4, client);
            Account.Account account2 = new DebitAccount(DateTime.Today.AddDays(2), bank2, 4, client);
            bank.Register(client, account);
            bank2.Register(client, account2);
            client.SetAddress("ulitsa");
            client.SetPassport("2323113");
            bool isAwareOfFirstBank = client.Accounts.First(t => t.NameOfBank == bank.Name) == account;
            bool isAwareOfSecondBank = client.Accounts.First(t => t.NameOfBank == bank2.Name) == account2;
            Assert.AreEqual(isAwareOfFirstBank, isAwareOfSecondBank);
        }

        [Test]
        public void ClientIsTryingToGetTooMuchMoneyFromAccount_ClientDoesntHaveMoney()
        {
            Bank bank = new Bank("Ilia's Bank");
            Client client = new Client("someone", "who");
            Account.Account account = new DebitAccount(DateTime.Today.AddDays(2), bank, 10, client);
            bank.ChangeLimitForShadyAccountsSettings(5);
            bank.Register(client, account);
            Assert.AreEqual(false, account.CanWithdraw(10));
        }
        
        [Test]
        public void DepositeIsNotAvalaibale_MoneyIsNotGet()
        {
            Bank bank1 = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            bank1.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank1.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            Account.Account account = new DepositAccount(DateTime.Today.AddYears(1), bank1, 100000, client);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank1});
            bank1.Register(client, account);
            Time.Instance.AddTime(0, 11, 1);
            Assert.AreEqual(0, (int) account.Money);
            Time.Instance.AddTime(0, 5, 0);
            Assert.AreEqual(true, (int) account.Money > 0);
        }

        [Test]
        public void DepositeAccountIsSet_AccountSetCorrectly()
        {
            Bank bank1 = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            bank1.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank1.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            Account.Account account = new DepositAccount(DateTime.Today.AddYears(1), bank1, 21, client);
            Assert.AreEqual(2.7f, account.Percent);
        }

        [Test]
        public void SomethinWentWrong_UndoAll()
        {
            Bank bank = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            bank.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            Account.Account account = new DebitAccount(DateTime.Today.AddYears(1), bank, 100000, client);
            bank.Register(client, account);
            Time.Instance.ResetTime();
            Time.Instance.AddTime(0, 0, 31);
            Assert.AreEqual(100310, (int) account.Money);
            account.UndoAll();
            Assert.AreEqual(100000, (int) account.Money);
        }
        [Test]
        public void SomethinWentWrong_UndoCommand()
        {
            Bank bank = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            bank.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            Account.Account account = new DebitAccount(DateTime.Today.AddYears(1), bank, 100000, client);
            bank.Register(client, account);
            Time.Instance.ResetTime();
            Time.Instance.AddTime(0, 0, 31);
            Assert.AreEqual(100310, (int) account.Money);
            account.UndoCommand(account.HistoryCommands[0]);
            Assert.AreEqual(100300, (int) account.Money);
        }
        [Test]
        public void TransferCommandExecuted_MoneyMoved()
        {
            Bank bank = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            bank.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            Account.Account account = new DebitAccount(DateTime.Today.AddYears(1), bank, 100010, client);
            Account.Account account1 = new DebitAccount(DateTime.Today.AddYears(1), bank, 100000, client);
            bank.Register(client, account);
            bank.Register(client, account1);
            decimal before = account.Money - account1.Money;
            account.TestFunc(new TransferCommand(account,account1,10));
            decimal after =account.Money - account1.Money;
            Assert.AreEqual(before ,-after);
        }
        [Test]
        public void WithdrawrCommandExecuted_MoneyMoved()
        {
            Bank bank = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            client.SetAddress("sdsd");
            client.SetPassport("111");
            bank.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            Account.Account account = new DebitAccount(DateTime.Today.AddYears(1), bank, 100010, client);
            bank.Register(client, account);
            decimal before = client.PocketMoney;
            account.TestFunc(new WithdrawCommand(account,10));
            Assert.AreEqual(before + 10 ,client.PocketMoney);
        }
        [Test]
        public void ReplenishCommandExecuted_MoneyMoved()
        {
            Bank bank = new Bank("ilia Bank");
            Client client = new Client("someon", "who");
            client.SetAddress("sdsd");
            client.SetPassport("111");
            bank.ChangeDebAccSettings(3.65f);
            List<decimal> moneyThresholds = new List<decimal>() {10, 20, 40};
            List<float> percentThresholds = new List<float>() {2.2f, 2.6f, 2.7f, 4f};
            bank.ChangeDepAccSettings(moneyThresholds, percentThresholds);
            CentralBank.Instance.AddBanks(new List<Bank>() {bank});
            Account.Account account = new DebitAccount(DateTime.Today.AddYears(1), bank, 0, client);
            bank.Register(client, account); 
            account.TestFunc(new ReplenishCommand(account,10));
            Assert.AreEqual(10d ,account.Money);
        }
    }
}