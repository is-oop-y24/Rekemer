using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Banks.Account;
using Microsoft.VisualBasic.CompilerServices;

namespace Banks
{
    public class Bank
    {
        private Dictionary<Client, List<Account.Account>> _accountsOfClients =
            new Dictionary<Client, List<Account.Account>>();

        // setting for debAccount

        public Bank(string name)
        {
            Name = name;
            UI = new UIBank(this);
        }

        public readonly UIBank UI;
        public float PercentDebAccount { get; private set; }
        public Vector3 ValidTimeDeb { get; private set; }

        // setting for depAccount
        public List<decimal> MoneyThresholdsDepAcc { get; set; } = new List<decimal>();

        public List<float> PercentThreshldsDepAcc { get; set; } = new List<float>();

        public Vector3 ValidTimeDep { get; private set; }

        // setting for credit account
        public decimal Limit { get; private set; }

        public decimal Commission { get; private set; }

        public string Name { get; private set; }

        public Vector3 ValidTimeCredit { get; private set; }


        // set settings for doubtful clients
        public decimal LimitForWithdrawAndTransfer { get; private set; }

        public void ChangeValidTimeCredit(int year, int month, int days)
        {
            ValidTimeCredit = new Vector3(year, month, days);
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification =
                $"in bank {Name} new duration for credit accounts: {ValidTimeCredit.X} years {ValidTimeCredit.Y} months {ValidTimeCredit.Z} days";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }


        public void ChangeValidTimeDeb(int year, int month, int days)
        {
            ValidTimeDeb = new Vector3(year, month, days);
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification =
                $"in bank {Name} new duration for debit accounts: {ValidTimeDeb.X} years {ValidTimeDeb.Y} months {ValidTimeDeb.Z} days";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }


        public void ChangeDebAccSettings(float percent)
        {
            PercentDebAccount = percent;
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification = $"in bank {Name} new percent for debit accounts: {PercentDebAccount} %";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }


        public void ChangeDepAccSettings(List<decimal> moneyThresholds, List<float> percentThresholds)
        {
            if (moneyThresholds.Count + 1 == percentThresholds.Count)
            {
                for (int i = 0; i < moneyThresholds.Count - 1; i++)
                {
                    if (moneyThresholds[i] > moneyThresholds[i + 1])
                    {
                        throw new Exception($"money thresholds are in wrong order in{this.Name}");
                    }
                }

                // change percent of current accounts
                MoneyThresholdsDepAcc = moneyThresholds;
                PercentThreshldsDepAcc = percentThresholds;
            }

            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification = $"in bank {Name} new conditions for deposit accounts:" + Environment.NewLine;
            for (int i = 0; i < moneyThresholds.Count; i++)
            {
                bool isLast = (i == moneyThresholds.Count - 1);
                notification += $"{percentThresholds[i]} : {(isLast ? ">" : "<")} {moneyThresholds[i]}" +
                                Environment.NewLine;
            }

            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }

        public void ChangeCredAccSettings(decimal limit, decimal commission)
        {
            Limit = limit;
            Commission = commission;
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification = $"in bank {Name} new limit and commission for creditAccounts: {limit} {commission}";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }

        public void ChangeLimitForShadyAccountsSettings(decimal limit)
        {
            LimitForWithdrawAndTransfer = limit;
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification = $"in bank {Name} new limit for users with not fully activated accounts: {limit}";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }


        public float DeterminePercent(decimal amount)
        {
            if (amount == 0) return 0f;
            var moneyThresholds = MoneyThresholdsDepAcc;
            var percentThresholds = PercentThreshldsDepAcc;
            int indOfAdjacent1 = -1;
            int indOfAdjacent2 = -1;
            if (moneyThresholds != null || percentThresholds != null)
            {
                for (int i = 0; i < moneyThresholds.Count; i++)
                {
                    if (amount > moneyThresholds[i])
                    {
                        if (i + 1 < moneyThresholds.Count)
                        {
                            if (amount < moneyThresholds[i + 1])
                            {
                                indOfAdjacent1 = i;
                                indOfAdjacent2 = i + 1;
                            }
                        }
                        else
                        {
                            indOfAdjacent1 = moneyThresholds.Count;
                        }
                    }
                }

                if (indOfAdjacent2 == -1 && indOfAdjacent1 == -1)
                {
                    return percentThresholds[0];
                }

                return percentThresholds[Math.Max(indOfAdjacent2, indOfAdjacent1)];
            }
            else
            {
                throw new Exception($"bank {Name} didnt set not set deposite settings");
            }
        }

        public void Register(Client client, Account.Account accountToRegister)
        {
            // check if null
            if (client == null)
            {
                return;
            }

            // check if already in data
            if (_accountsOfClients.ContainsKey(client))
            {
                _accountsOfClients[client].Add(accountToRegister);
                return;
            }

            // if not then register  account with certain status
            _accountsOfClients.Add(client, new List<Account.Account>());
            _accountsOfClients[client].Add(accountToRegister);
        }

        public void Update()
        {
            foreach (var accountsOfClient in _accountsOfClients)
            {
                foreach (Account.Account account in accountsOfClient.Value)
                {
                    account.Update();
                }
            }
        }

        public Client IsUserExist(string name, string surname)
        {
            if (_accountsOfClients == null) return null;
            foreach (var key in _accountsOfClients.Keys)
            {
                if (key.Surname == surname && key.Name == name)
                {
                    return key;
                }
            }

            return null;
        }
        
        public Client IsUserExist(string id)
        {
            if (_accountsOfClients == null) return null;
            foreach (var client in _accountsOfClients.Keys)
            {
                if (client.id.ToString() == id)
                {
                    return client;
                }
            }

            return null;
        }

        
        public List<Account.Account> GetAllAccounts()
        {
            List<Account.Account> accounts = new List<Account.Account>();
            foreach (var key in _accountsOfClients.Keys)
            {
             
                accounts = accounts.Union(_accountsOfClients[key]).ToList();
            }

            return accounts;

        }
        public List<Account.Account> GetAccountsOfUser(Client client)
        {
            List<Account.Account> accounts = new List<Account.Account>();
            if (_accountsOfClients.ContainsKey(client))
            {
                if (_accountsOfClients[client] != null)
                {
                    accounts = _accountsOfClients[client];
                }
            }

            return accounts;
        }
    }
}