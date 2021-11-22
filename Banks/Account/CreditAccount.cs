using System;
using System.Collections.Generic;
using Banks.Command;

namespace Banks.Account
{
    public class CreditAccount : Account
    {
        private double _commisiion;
        public override double Money { get; }
        protected override float _percent { get; set; }

        public override void AddMoney(double money)
        {
            _money += money;
        }

        public override bool CanWithdraw(double money)
        {
            if (money < Money + Limit)
            {
                return true;
            }

            return false;
        }

        public override void Update()
        {
            if (Time.Instance.CurrentTime >= EndTime)
            {
                return;
            }
        }

        public override bool Update(double money)
        {
            Command.Command command = new ReplenishCommand(this, money);
            CommandsToExecuteInTheEndOfMonth.Add(command);
            return true;
        }

        protected override double CalculateMoneyWithProcents(int days)
        {
            return 0d;
        }

        public override void Menu()
        {
            // show status and let change something
            Console.WriteLine("ownerInfo:");
            Console.WriteLine(Client.Status);
            if (Client.Status == ClientStatus.Dubious)
            {
                Console.WriteLine($"you status is" + Client.Status);
                Console.WriteLine("Please check you passport and address data in client Settings. Do you want to do it now? y/n");
                if (Console.ReadLine() == "y")
                {
                    // let him go and check
                    Client.Menu(_bank);
                }
            }

            Console.WriteLine($"activation date: {StartTime}");
            Console.WriteLine($"expiration date: {EndTime}");
            Console.WriteLine($"money: {_money} will be available in {EndTime}");

            while (true)
            {
                Console.WriteLine("Do you want to replenish(1), Withdraw(2) or transfer(3) or quit(q)? ");
                //let choose command
                string input = Console.ReadLine();
                if (input == "1")
                {
                    Console.WriteLine("how much money do you want to add?");
                    double money = 0;
                    while (true)
                    {
                        money = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine($"is {money} ok to add? y/n");
                        input = Console.ReadLine();
                        if (input == "y")
                        {
                            break;
                        }
                        else if (input == "n")
                        {
                        }
                    }

                    if (Update(money))
                    {
                        Console.WriteLine("Operation is Successful");
                        Client.AddMoney(-money);
                        Console.WriteLine($"Your pocket money balance is {Client.PocketMoney}");
                    }

                    ;
                }
                else if (input == "2")
                {
                    double money = 0;
                    if (Client.Status == ClientStatus.Dubious)
                    {
                        Console.WriteLine(
                            "You have not fully activated your profile - fill your address and passport fields in settings");
                        Console.WriteLine(
                            $"Due to status of your account, you have limit for withdrawing money from account {_bank.LimitForWithdrawAndTransfer}");
                        Console.WriteLine("how much money do you want to withdraw?");


                        while (true)
                        {
                            money = Convert.ToDouble(Console.ReadLine());
                            if (money >= _bank.LimitForWithdrawAndTransfer)
                            {
                                Console.WriteLine("you cannot get this money from account since there is limit set");
                                continue;
                            }

                            break;
                        }

                        if (money != 0)
                        {
                            if (CanWithdraw(money))
                            {
                                if (money >= _money)
                                {
                                    if (_commisiion <= Client.PocketMoney)
                                    {
                                        Client.AddMoney(-_commisiion);
                                        CommandProccesor.ExecuteCommand(new WithdrawCommand(this, money));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Not enough money for commission");
                                    }

                                    CommandProccesor.ExecuteCommand(new WithdrawCommand(this, money));
                                }
                            }
                            else Console.WriteLine("not enough money on account");
                        }
                    }
                    else if (Client.Status == ClientStatus.Full)
                    {
                        money = Convert.ToDouble(Console.ReadLine());
                        if (CanWithdraw(money))
                        {
                            if (money >= _money)
                            {
                                if (_commisiion <= Client.PocketMoney)
                                {
                                    Client.AddMoney(-_commisiion);
                                    CommandProccesor.ExecuteCommand(new WithdrawCommand(this, money));
                                }
                                else
                                {
                                    Console.WriteLine("Not enough money fpr commission");
                                }

                                CommandProccesor.ExecuteCommand(new WithdrawCommand(this, money));
                            }
                        }
                        else Console.WriteLine("not enough money on account");
                    }
                }
                else if (input == "3")
                {
                    List<Account> accounts = _bank.GetAccountsOfUser(Client);
                    int i = 0;
                    if (accounts == null)
                    {
                        Console.WriteLine("There is no any account of this user or he is not registered");
                        return;
                    }

                    foreach (var account in accounts)
                    {
                        Console.WriteLine($"{i}) " + account);
                        i++;
                    }

                    Console.WriteLine("Choose account");
                    int number = 0;
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
                        }
                    }

                    Account accountToTransfer = accounts[number];
                    Console.WriteLine($"How much money do you want to transfer from {this} to {accountToTransfer}");
                    double money = 0;
                    while (true)
                    {
                        money = Convert.ToDouble(Console.ReadLine());
                        if (Client.Status == ClientStatus.Dubious)
                        {
                            if (money >= _bank.LimitForWithdrawAndTransfer)
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
                        }
                        else if (input == "q")
                        {
                        }
                    }

                    // check input money and if transatcion is possible

                    if (CanWithdraw(money))
                    {
                        if (money >= _money)
                        {
                            if (_commisiion <= Client.PocketMoney)
                            {
                                Client.AddMoney(-_commisiion);
                                var transferCommand = new TransferCommand(this, accountToTransfer, money);
                                CommandProccesor.ExecuteCommand(transferCommand);
                            }
                            else
                            {
                                Console.WriteLine("Not enough money for commission");
                            }

                            CommandProccesor.ExecuteCommand(new TransferCommand(this, accountToTransfer, money));
                        }
                    }
                    else Console.WriteLine("Operation is not possible since account is not expired yet");
                }
                else if (input == "q")
                {
                    _bank.Menu();
                    return;
                }
                else
                {
                    Console.WriteLine("wrong input");
                }
            }
        }

        public override void UpdatePercent(float percent)
        {
            return;
        }


        public CreditAccount(DateTime endTime, Bank bank, double money, double commission, Client client) : base(
            endTime, bank, money,
            client)
        {
            _commisiion = commission;
        }
    }
}