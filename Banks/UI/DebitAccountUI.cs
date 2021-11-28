using System;
using System.Collections.Generic;
using Banks.Account;
using Banks.Command;

namespace Banks.UI
{
    public class DebitAccountUI : AccountUI
    {
        public DebitAccountUI(DebitAccount account) : base(account)
        {
        }

        public override void Menu()
        {
            // show status and let change something
            Console.WriteLine("ownerInfo:");
            Console.WriteLine(_account.Client.Status);
            if (_account.Client.Status == ClientStatus.Dubious)
            {
                Console.WriteLine($"you status is " + _account.Client.Status);
                Console.WriteLine(
                    "Please check you passport and address data in client Settings. Do you want to do it now? y/n");
                if (Console.ReadLine() == "y")
                {
                    // let him go and check
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowClientMenu(new ClientUI(_account.Client),_account.Bank);
                    }

                }
            }

            Console.WriteLine($"activation date: {_account.StartTime}");
            Console.WriteLine($"expiration date: {_account.EndTime}");
            Console.WriteLine($"percent: {_account.Bank.PercentDebAccount}");
            Console.WriteLine($"money: {_account.Money}");

            while (true)
            {
                Console.WriteLine("Do you want to replenish(1), Withdraw(2) or transfer(3) or quit(q)? ");
                //let choose command
                string input = Console.ReadLine();
                if (input == "1")
                {
                    decimal money = 0;
                    while (true)
                    {
                        Console.WriteLine("how much money do you want to add?");
                        money = Convert.ToDecimal(Console.ReadLine());
                        if (money > _account.Client.PocketMoney)
                        {
                            Console.WriteLine("You dont have so much money");
                            continue;
                        }

                        Console.WriteLine($"is {money} ok to add? y/n or q");
                        input = Console.ReadLine();
                        if (input == "y")
                        {
                            break;
                        }
                        else if (input == "n")
                        {
                            continue;
                        }
                        else if (input == "q")
                        {
                            money = 0;
                        }
                    }

                    if (money != 0)
                    {
                        if (_account.Update(money))
                        {
                            Console.WriteLine("Operation is Successful");
                            _account.Client.Substract(-money);
                            Console.WriteLine($"Your pocket money balance is {_account.Client.PocketMoney}");
                        }
                    }
                    else
                    {
                        Menu();
                    }
                }
                else if (input == "2")
                {
                    decimal money = 0;
                    if (_account.Client.Status == ClientStatus.Dubious)
                    {
                        Console.WriteLine(
                            "You have not fully activated your profile - fill your address and passport fields in settings");
                        Console.WriteLine(
                            $"Due to status of your account, you have limit for withdrawing money from account {_account.Bank.LimitForWithdrawAndTransfer}");


                        while (true)
                        {
                            Console.WriteLine("how much money do you want to withdraw?");
                            money = Convert.ToDecimal(Console.ReadLine());
                            if (money >= _account.Bank.LimitForWithdrawAndTransfer)
                            {
                                Console.WriteLine("you cannot get this money from account since there is limit set");
                                money = 0;
                                continue;
                            }

                            break;
                        }

                        if (money != 0)
                        {
                            _account.CommandProccesor.ExecuteCommand(new WithdrawCommand(_account, money));
                            Console.WriteLine("Operation is Successful");
                        }
                    }
                    else if (_account.Client.Status == ClientStatus.Full)
                    {
                        money = Convert.ToDecimal(Console.ReadLine());
                        _account.CommandProccesor.ExecuteCommand(new WithdrawCommand(_account, money));
                        Console.WriteLine("Operation is Successful");
                    }
                }
                else if (input == "3")
                {
                    List<Account.Account> accounts = _account.Bank.GetAccountsOfUser(_account.Client);
                    Account.Account accountToChoose = null;

                    Console.WriteLine("You wish to transfer money to different account? y/n");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        Console.WriteLine("enter id of account");
                        var id = Console.ReadLine();
                        var account = CentralBank.Instance.FindAccount(id);
                        if (account != null)
                        {
                            accountToChoose = account;
                        }

                        Console.WriteLine("Sorry, There is no such account");
                        Menu();
                    }


                    if (accounts == null)
                    {
                        Console.WriteLine("There is no any account of this user or he is not registered");
                        Menu();
                        break;
                    }

                    int i = 0;
                    foreach (var account in accounts)
                    {
                        Console.WriteLine($"{i}) " + account);
                        i++;
                    }

                    int number = 0;
                    if (accountToChoose == null)
                    {
                        Console.WriteLine("Choose account");

                        while (true)
                        {
                            number = Convert.ToInt32(Console.ReadLine());
                            if (number > accounts.Count)
                            {
                                Console.WriteLine("there is no such account");
                                continue;
                            }

                            Console.WriteLine("Are you sure ? y/n");
                            input = Console.ReadLine();
                            if (input == "y")
                            {
                                break;
                            }
                            else if (input == "n")
                            {
                                continue;
                            }
                        }
                    }

                    Account.Account accountToTransfer = null;
                    if (accountToChoose != null)
                    {
                        accountToTransfer = accountToChoose;
                    }
                    else
                    {
                        if (accounts[number] != null)
                        {
                            accountToTransfer = accounts[number];
                        }
                    }

                    Console.WriteLine($"How much money do you want to transfer from {_account} to {accountToTransfer}");
                    decimal money = 0;
                    while (true)
                    {
                        money = Convert.ToDecimal(Console.ReadLine());
                        if (_account.Client.Status == ClientStatus.Dubious)
                        {
                            if (money >= _account.Bank.LimitForWithdrawAndTransfer)
                            {
                                Console.WriteLine("you cannot get this money from account since there is limit set");
                                continue;
                            }
                        }

                        Console.WriteLine("Are you sure ? y/n or q(to quit)");
                        if (input == "y")
                        {
                            break;
                        }
                        else if (input == "n")
                        {
                            continue;
                        }
                        else if (input == "q")
                        {
                            Menu();
                        }
                    }

                    // check input money and if transatcion is possible
                    var transferCommand = new TransferCommand(_account, accountToTransfer, money);
                    _account.CommandProccesor.ExecuteCommand(transferCommand);
                    Console.WriteLine("Operation Successful");
                }
                else if (input == "q")
                {
                    
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowBankMenu(new UIBank(_account.Bank));
                    }
                    return;
                }
                else
                {
                    Console.WriteLine("wrong input");
                }
            }
        }
    }
}