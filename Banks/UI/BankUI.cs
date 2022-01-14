using System;
using System.Collections.Generic;
using Banks.Account;

namespace Banks.UI
{
#pragma warning disable SA1649
    public class UIBank
#pragma warning restore SA1649
    {
        private Bank _bank;

        public UIBank(Bank bank)
        {
            _bank = bank;
        }

        public List<Account.Account> ShowAccounts(Client client)
        {
            var accounts = _bank.GetAccountsOfUser(client);
            int i = 0;
            foreach (var account in accounts)
            {
                Console.WriteLine($"{i}) " + account);
                i++;
            }

            return accounts;
        }

        public void Menu()
        {
            // check if user already is logged in
            Console.WriteLine("please, input your name");
            string name = Console.ReadLine();
            Console.WriteLine("please, input your surname");
            string surname = Console.ReadLine();
            var user = _bank.IsUserExist(name, surname);
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
#pragma warning disable SA1123
            #region ShowingConditionAccountToUser
#pragma warning restore SA1123

            Console.WriteLine("Debit account condition: ");
            Console.WriteLine(_bank.PercentDebAccount);

#pragma warning disable SA1123
            #region ShowingDepositAccountCondition
#pragma warning restore SA1123

            var moneyThresholds = _bank.MoneyThresholdsDepAcc;
            var percentThresholds = _bank.PercentThreshldsDepAcc;
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
            Console.WriteLine($"Limit of spending {_bank.Limit}");
            Console.WriteLine($"Commission of spending limit {_bank.Commission}");

            #endregion

            while (true)
            {
                Console.WriteLine($"Would you like to get account in {_bank.Name}? y/n");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    var client = ReadClientsInput();

                    // which account will user get ?
                    Account.Account account = null;
                    Console.WriteLine($"1 - debit, 2 - credit, 3 - deposit?");
                    input = Console.ReadLine();

                    bool isChooseMoney = false;
                    decimal amount = 0;
                    while (!isChooseMoney)
                    {
                        Console.WriteLine("How much money will you keep there?");
                        amount = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine($"{amount}, are you sure? y/n");
                        input = Console.ReadLine();
                        if (input == "y")
                        {
                            isChooseMoney = true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (input == "1")
                    {
                        DateTime endTime = DateTime.Now;
                        endTime = endTime.AddYears((int)_bank.ValidTimeDeb.X).AddMonths((int)_bank.ValidTimeDeb.Y)
                            .AddDays((int)_bank.ValidTimeDeb.Z);
                        account = new DebitAccount(endTime, _bank, amount, client);
                    }
                    else if (input == "2")
                    {
                        DateTime endTime = DateTime.Now;
                        endTime = endTime.AddYears((int)_bank.ValidTimeCredit.X)
                            .AddMonths((int)_bank.ValidTimeCredit.Y)
                            .AddDays((int)_bank.ValidTimeCredit.Z);

                        account = new CreditAccount(endTime, _bank, amount, client);
                    }
                    else if (input == "3")
                    {
                        DateTime endTime = DateTime.Now;
                        endTime = endTime.AddYears((int)_bank.ValidTimeDep.X).AddMonths((int)_bank.ValidTimeDep.Y)
                            .AddDays((int)_bank.ValidTimeDep.Z);
                        float percent = _bank.DeterminePercent(amount);
                        account = new DepositAccount(endTime, _bank, amount, client);
                    }

                    if (account != null && client != null)
                    {
                        // register
                        _bank.Register(client, account);
                    }
                }
                else if (input == "q" || input == "n")
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.BanksMenu();
                    }
                }
            }
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
                string name = string.Empty;
                string surname = string.Empty;
                string address = string.Empty;
                string passport = string.Empty;

#pragma warning disable SA1123
                #region readingName
#pragma warning restore SA1123

                while (!isNameOk)
                {
                    Console.WriteLine("please, input your name");
                    name = Console.ReadLine();
                    Console.WriteLine($"is {name} good name for you? y/n or q to exit");
                    input = Console.ReadLine();
                    if (input == "q")
                    {
                        name = string.Empty;
                        break;
                    }

                    if (name != string.Empty)
                    {
                        if (input == "y")
                        {
                            isNameOk = true;
                            break;
                        }
                        else if (input == "n")
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("name is empty, input name please");
                    }
                }

                #endregion

#pragma warning disable SA1123
                #region readingSurname
#pragma warning restore SA1123

                while (!isSurnameOk)
                {
                    Console.WriteLine("please, input your surname");
                    surname = Console.ReadLine();
                    Console.WriteLine($"is {surname} good surname for you? y/n or q to exit");
                    input = Console.ReadLine();
                    if (input == "q")
                    {
                        surname = string.Empty;
                        break;
                    }

                    if (surname != string.Empty)
                    {
                        if (input == "y")
                        {
                            isSurnameOk = true;
                            break;
                        }
                        else if (input == "n")
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("surname is empty, input surname please");
                    }
                }

                #endregion

#pragma warning disable SA1123
                #region readingOptionalData
#pragma warning restore SA1123

                while (!isAddressOk && !isPassportOk)
                {
                    Console.WriteLine("to have unlimited account, please input your address and passport [OPTIONAL]");
                    Console.WriteLine("Do you want to have full status of your account? y/n ");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
#pragma warning disable SA1123
                        #region readingAddress
#pragma warning restore SA1123

                        while (!isAddressOk)
                        {
                            Console.WriteLine("please, input your address");
                            address = Console.ReadLine();
                            Console.WriteLine($"is {address} good address for you? y/n or q to exit");
                            input = Console.ReadLine();
                            if (input == "q")
                            {
                                address = string.Empty;
                                break;
                            }

                            if (address != string.Empty)
                            {
                                if (input == "y")
                                {
                                    isAddressOk = true;
                                    break;
                                }
                                else if (input == "n")
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine("address is empty, input address please");
                            }
                        }

                        #endregion

#pragma warning disable SA1123
                        #region readingPassport
#pragma warning restore SA1123

                        while (!isPassportOk)
                        {
                            Console.WriteLine("please, input your passport ");
                            passport = Console.ReadLine();
                            Console.WriteLine($"is {passport} good passport for you? y/n or q to exit");
                            input = Console.ReadLine();
                            if (input == "q")
                            {
                                passport = string.Empty;
                                break;
                            }

                            if (passport != string.Empty)
                            {
                                if (input == "y")
                                {
                                    isPassportOk = true;
                                    break;
                                }
                                else if (input == "n")
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine("passport is empty, input passport please");
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
                if (input == "q")
                {
                    break;
                }

                if (input == "y")
                {
                    Client client = new Client(name, surname);
                    if (address != string.Empty)
                    {
                        client.SetAddress(address);
                    }

                    if (passport != string.Empty)
                    {
                        client.SetPassport(passport);
                    }
                }
                else if (input == "n")
                {
                    continue;
                }
            }

            return null;
        }

        private void ChooseAccount(List<Account.Account> accounts, int nAccount)
        {
            if (accounts == null) return;
            if (nAccount > accounts.Count) return;
            var account = accounts[nAccount];
            UIManager.Instance.ShowAccountMenu(account.DeriveUI());
        }
    }
}