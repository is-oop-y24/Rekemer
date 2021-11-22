using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Banks.Account;
using Microsoft.VisualBasic.CompilerServices;

namespace Banks
{
    public class Bank
    {
        private Dictionary<Client, List<Account.Account>> _accountsOfClients = new Dictionary<Client, List<Account.Account>>();

        // setting for debAccount

        public Bank(string name)
        {
            Name = name;
        }

        public float PercentDebAccount { get; private set; }
        public Vector3 validTimeDeb { get; private set; }

        // setting for depAccount
        public List<double> MoneyThresholdsDepAcc { get; set; } = new List<double>();

        public List<float> PercentThreshldsDepAcc { get; set; } = new List<float>();

        public Vector3 validTimeDep { get; private set; }

        // setting for credit account
        public double Limit { get; private set; }

        public double Commission { get; private set; }

        public string Name { get; private set; }

        public Vector3 validTimeCredit { get; private set; }


        // set settings for doubtful clients
        public double LimitForWithdrawAndTransfer { get; private set; }

        public void CnahgevValidTimeCredit(int year, int month, int days)
        {
            validTimeCredit = new Vector3(year, month, days);
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification =
                $"in bank {Name} new duration for credit accounts: {validTimeCredit.X} years {validTimeCredit.Y} months {validTimeCredit.Z} days";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }


        public void CnahgeValidTimeDeb(int year, int month, int days)
        {
            validTimeDeb = new Vector3(year, month, days);
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification =
                $"in bank {Name} new duration for debit accounts: {validTimeDeb.X} years {validTimeDeb.Y} months {validTimeDeb.Z} days";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }

        public void CnahgeValidTimeDep(int year, int month, int days)
        {
            validTimeDep = new Vector3(year, month, days);
            // notify
            var clients = new List<Client>(_accountsOfClients.Keys);
            string notification =
                $"in bank {Name} new duration for deposit accounts: {validTimeDep.X} years {validTimeDep.Y} months {validTimeDep.Z} days";
            foreach (var client in clients)
            {
                client.Update(notification);
            }
        }

        public void CnahgeDebAccSetitings(float percent)
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


        public void CnahgeDepAccSetitings(List<double> moneyThresholds, List<float> percentThresholds)
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

                MoneyThresholdsDepAcc = moneyThresholds;
                PercentThreshldsDepAcc = percentThresholds;
                // change percent of current accounts
                foreach (KeyValuePair<Client, List<Account.Account>> accountsOfClient in _accountsOfClients)
                {
                    foreach (Account.Account account in accountsOfClient.Value)
                    {
                        if (account is DepositAccount)
                        {
                            account.UpdatePercent(DeterminePercent(account.Money));
                        }
                    }
                }
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

        public void CnahgeCredAccSetitings(double limit, double commission)
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

        public void CnahgeLimitForShadyAccountsSettings(double limit)
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


        public float DeterminePercent(double amount)
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

        public List<Account.Account> ShowAccounts(Client client)
        {
            var accounts = GetAccountsOfUser(client);
            int i = 0;
            foreach (var account in accounts)
            {
                Console.WriteLine($"{i}) " + account);
                i++;
            }

            return accounts;
        }

        public Client ReadClientsInput()
        {
            while (true)
            {
                bool isNameOk = false;
                bool isSurnameOk = false;
                bool isAddressOk = false;
                bool isPassportOk = false;
                string input;
                string name = String.Empty;
                string surname = String.Empty;
                string address = String.Empty;
                string passport = String.Empty;

                #region readingName

                while (!isNameOk)
                {
                    Console.WriteLine("please, input your name");
                    name = Console.ReadLine();
                    Console.WriteLine($"is {name} good name for you? y/n or q to exit");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        isNameOk = true;
                        break;
                    }
                    else if (input == "n")
                    {
                        continue;
                    }
                    else if (input == "q")
                    {
                        name = String.Empty;
                        break;
                    }
                }

                #endregion

                #region readingSurname

                while (!isSurnameOk)
                {
                    Console.WriteLine("please, input your surname");
                    surname = Console.ReadLine();
                    Console.WriteLine($"is {surname} good surname for you? y/n or q to exit");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        isSurnameOk = true;
                        break;
                    }
                    else if (input == "n")
                    {
                        continue;
                    }
                    else if (input == "q")
                    {
                        surname = string.Empty;
                        break;
                    }
                }

                #endregion

                #region readingOptionalData

                while (!isAddressOk && !isPassportOk)
                {
                    Console.WriteLine("to have unlimited account, please input your address and passport [OPTIONAL]");
                    Console.WriteLine("Do you want to have full status of your account? y/n ");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        #region readingAddress

                        while (!isAddressOk)
                        {
                            Console.WriteLine("please, input your address");
                            address = Console.ReadLine();
                            Console.WriteLine($"is {address} good address for you? y/n or q to exit");
                            input = Console.ReadLine();
                            if (input == "y")
                            {
                                isAddressOk = true;
                            }
                            else if (input == "n")
                            {
                                continue;
                            }
                            else if (input == "q")
                            {
                                address = String.Empty;
                                break;
                            }
                        }

                        #endregion

                        #region readingPassport

                        while (!isPassportOk)
                        {
                            Console.WriteLine("please, input your passport ");
                            passport = Console.ReadLine();
                            Console.WriteLine($"is {address} good passport for you? y/n or q to exit");
                            input = Console.ReadLine();
                            if (input == "y")
                            {
                                isPassportOk = true;
                            }
                            else if (input == "n")
                            {
                                continue;
                            }
                            else if (input == "q")
                            {
                                passport = String.Empty;
                                break;
                            }
                        }

                        #endregion
                    }
                    else if (input == "n")
                    {
                        break;
                    }
                }

                #endregion

                // check if data is correct
                Console.WriteLine("is your data correct? y/n or q to exit");
                Console.WriteLine("name: " + name);
                Console.WriteLine("surname: " + surname);
                Console.WriteLine("address: " + address);
                Console.WriteLine("passport: " + passport);
                input = Console.ReadLine();
                if (input == "y")
                {
                    Client client = new Client(name, surname);
                    if (address != String.Empty)
                    {
                        client.SetAddress(address);
                    }

                    if (passport != String.Empty)
                    {
                        client.SetPassport(passport);
                    }
                }
                else if (input == "n")
                {
                    continue;
                }
                else if (input == "q")
                {
                    break;
                }
            }

            return null;
        }

        public Client CheckExistenceOfUser(string name, string surname)
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

        private void ChooseAccount(List<Account.Account> accounts, int nAccount)
        {
            if (accounts == null) return;
            if (nAccount > accounts.Count) return;
            var account = accounts[nAccount];
            account.Menu();
        }

        public void Menu()
        {
            // check if user already is logged in
            Console.WriteLine("please, input your name");
            string name = Console.ReadLine();
            Console.WriteLine("please, input your surname");
            string surname = Console.ReadLine();
            var user = CheckExistenceOfUser(name, surname);
            if (user != null)
            {
                // show his accounts
                int input;
                var accounts = ShowAccounts(user);
                while (true)
                {
                    Console.WriteLine("Which account do you want to check?");
                    input = Convert.ToInt32(Console.ReadLine());
                    if (input >= accounts.Count)
                    {
                        Console.WriteLine("There is no such option, try another number");
                    }
                    else
                    {
                        break;
                    }
                }

                ChooseAccount(accounts, input);
            }

            // get information about bank

            #region ShowingConditionAccountToUser

            Console.WriteLine("Debit account condition: ");
            Console.WriteLine(PercentDebAccount);

            #region ShowingDepositAccountCondition

            var moneyThresholds = MoneyThresholdsDepAcc;
            var percentThresholds = PercentThreshldsDepAcc;
            if (moneyThresholds != null && percentThresholds != null)
            {
                for (int i = 0; i < moneyThresholds.Count; i++)
                {
                    if (i > 0 && i != moneyThresholds.Count - 1)
                    {
                        Console.WriteLine(
                            $"{percentThresholds[i]}: {moneyThresholds[i - 1]} < deposit < {moneyThresholds[i]}");
                    }
                    else if (i == moneyThresholds.Count - 1)
                    {
                        Console.WriteLine($"{percentThresholds[i]}: deposit > {moneyThresholds[i]}");
                    }
                    else
                    {
                        Console.WriteLine($"{percentThresholds[i]}: deposit < {moneyThresholds[i]}");
                    }
                }
            }

            #endregion


            Console.WriteLine($"Credit account condition: ");
            Console.WriteLine($"Limit of spending {Limit}");
            Console.WriteLine($"Commission of spending limit {Commission}");

            #endregion

            while (true)
            {
                Console.WriteLine($"Would you like to get account in {Name}? y/n");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    var client = ReadClientsInput();
                    // which account will user get ?
                    Account.Account account = null;
                    Console.WriteLine($"1 - debit, 2 - credit, 3 - deposit?");
                    input = Console.ReadLine();

                    bool isChooseMoney = false;
                    double amount = 0;
                    while (!isChooseMoney)
                    {
                        Console.WriteLine("How much money will you keep there?");
                        amount = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine($"{amount}, are you sure? y/n");
                        input = Console.ReadLine();
                        if (input == "y")
                        {
                            isChooseMoney = true;
                        }
                        else continue;
                    }

                    if (input == "1")
                    {
                        DateTime endTime = DateTime.Now;
                        endTime = endTime.AddYears((int) validTimeDeb.X).AddMonths((int) validTimeDeb.Y)
                            .AddDays((int) validTimeDeb.Z);
                        account = new DebitAccount(endTime, this, amount, client);
                    }
                    else if (input == "2")
                    {
                        DateTime endTime = DateTime.Now;
                        endTime = endTime.AddYears((int) validTimeCredit.X).AddMonths((int) validTimeCredit.Y)
                            .AddDays((int) validTimeCredit.Z);

                        account = new CreditAccount(endTime, this, amount, Commission, client);
                    }
                    else if (input == "3")
                    {
                        DateTime endTime = DateTime.Now;
                        endTime = endTime.AddYears((int) validTimeDep.X).AddMonths((int) validTimeDep.Y)
                            .AddDays((int) validTimeDep.Z);
                        float percent = DeterminePercent(amount);
                        account = new DepositAccount(endTime, this, amount, client);
                    }

                    if (account != null && client != null)
                    {
                        // register
                        Register(client, account);
                    }
                }
                else if (input == "q" || input == "n")
                {
                    ConsoleUI.Instance.BanksMenu();
                }
            }
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