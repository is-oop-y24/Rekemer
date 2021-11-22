using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Command;

namespace Banks
{
    public class DepositAccount : Account.Account
    {
        private DateTime _lastUpdate;
        private bool canGetOutside;

        public DepositAccount(DateTime endTime, Bank bank, double money, Client client) : base(endTime,
            bank, money, client)
        {
        }

        public override double Money
        {
            get
            {
                if (canGetOutside)
                {
                    return _money;
                }

                return 0;
            }
        }

        protected override float _percent
        {
            get => _bank.DeterminePercent(_money);
            set => _percent = value;
        }

        public override void AddMoney(double money)
        {
            _money += money;
        }

        public override bool CanWithdraw(double money)
        {
            if (Time.Instance.CurrentTime >= EndTime)
            {
                if (money < Money)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Update()
        {
            if (Time.Instance.CurrentTime >= EndTime)
            {
                // close or something
                canGetOutside = true;
                return;
            }

            // find out how many days are passed
            var passedDays = (Time.Instance.CurrentTime - StartTime).Days;
            if (Math.Abs(Time.Instance.CurrentTime.Day - _lastUpdate.Day) == 1) _isBonusMoney = true;
            // consider all passed days
            for (int i = 0; i < passedDays; i++)
            {
                Command.Command command = new ReplenishCommand(this,  CalculateMoneyWithProcents(1));
                CommandsToExecuteInTheEndOfMonth.Add(command);
            }
            
            List<Command.Command> commandsToDelete = new List<Command.Command>();
            foreach (var comm in CommandsToExecuteInTheEndOfMonth)
            {
                if (passedDays >= 31)
                {
                    CommandProccesor.ExecuteCommand(comm);
                    commandsToDelete.Add(comm);
                }
            }

            CommandsToExecuteInTheEndOfMonth = CommandsToExecuteInTheEndOfMonth.Except(commandsToDelete).ToList();
            _lastUpdate = Time.Instance.CurrentTime;
        }

        public override bool Update(double money)
        {
            double increase = money / 100 * _percent / 365;
            Command.Command command = new ReplenishCommand(this,  increase);
            CommandsToExecuteInTheEndOfMonth.Add(command);
            return true;
        }


        protected override double CalculateMoneyWithProcents(int days)
        {
            return _money * _bank.PercentDebAccount / (100f * 365f);
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
            Console.WriteLine($"percent: {_percent}");
            Console.WriteLine($"money: {Money}");

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
                            }

                            break;
                        }

                        if (money != 0)
                        {
                            if (CanWithdraw(money))
                            {
                                CommandProccesor.ExecuteCommand(new WithdrawCommand(this, 
                                    money));
                                Console.WriteLine("Operation is Successful");
                            }
                            else Console.WriteLine("Operation is not possible since account is not expired yet");
                        }
                    }
                    else if (Client.Status == ClientStatus.Full)
                    {
                        money = Convert.ToDouble(Console.ReadLine());
                        if (CanWithdraw(money))
                        {
                            CommandProccesor.ExecuteCommand(new WithdrawCommand(this, 
                                money));
                            Console.WriteLine("Operation is Successful");
                        }
                        else Console.WriteLine("Operation is not possible since account is not expired yet");
                    }
                }
                else if (input == "3")
                {
                    List<Account.Account> accounts = _bank.GetAccountsOfUser(Client);
                    int i = 0;
                    if (accounts == null)
                    {
                        Console.WriteLine("There is no any account of this user or he is not registered");
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

                    Account.Account accountToTransfer = accounts[number];
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
                        var transferCommand =
                            new TransferCommand(this, accountToTransfer,  money);
                        CommandProccesor.ExecuteCommand(transferCommand);
                        Console.WriteLine("Operation is Successful");
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
            this._percent = percent;
        }
    }
}