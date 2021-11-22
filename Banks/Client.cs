using System;
using System.Collections.Generic;

namespace Banks
{
    public class Client
    {
        private string _address = String.Empty;
        private string _passport = String.Empty;
        private ClientStatus _status = ClientStatus.Dubious;
        private List<Account.Account> _accounts = new List<Account.Account>();
        public double PocketMoney { get; private set; }

        public List<Account.Account> Accounts
        {
            get => _accounts;
        }

        public ClientStatus Status
        {
            get => _status;
            private set => _status = value;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string Passport
        {
            get => _passport;
            set => _passport = value;
        }

        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public void SetPassport(string passport)
        {
            Passport = passport;
            if (Address != String.Empty)
            {
                Status = ClientStatus.Full;
            }
        }

        public void Update(string notification)
        {
            // do something with this notification - show on his phone, client's accounts are  already updated  at this point
        }

        public void SetAddress(string Address)
        {
            this.Address = Address;
            if (Passport != String.Empty)
            {
                Status = ClientStatus.Full;
            }
        }

        public void AddMoney(double money)
        {
            PocketMoney += money;
        }

        public void AddAccount(Account.Account account)
        {
            _accounts.Add(account);
        }

        public void Menu(Bank bank)
        {
            Console.WriteLine("welcome to client menu");
            Console.WriteLine("1) your surname: " + Surname);
            Console.WriteLine("2) your name: " + Name);
            Console.WriteLine("3) your address: " + Address);
            Console.WriteLine("4) your passport: " + Passport);
            Console.WriteLine("Do you want to change something? y/ or q to exit");
            string input = Console.ReadLine();
            while (true)
            {
                if (input == "y")
                {
                    Console.WriteLine("Which field?");
                    input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            while (true)
                            {
                                Console.WriteLine(" your new surname is");
                                input = Console.ReadLine();
                                var newUserData = input;
                                Console.WriteLine($" your new surname {newUserData} is ok? y/n or q");
                                input = Console.ReadLine();
                                if (input == "q")
                                {
                                    bank.Menu();
                                    break;
                                }
                                else if (input == "y")
                                {
                                    // save new data
                                    Surname = newUserData;
                                    break;
                                }
                                else if (input == "n")
                                {
                                }
                                else
                                {
                                    Console.WriteLine("wrong input");
                                }
                            }

                            break;
                        case "2":
                            while (true)
                            {
                                Console.WriteLine(" your new name is");
                                input = Console.ReadLine();
                                var newUserData = input;
                                Console.WriteLine($" your new name {newUserData} is ok? y/n or q");
                                input = Console.ReadLine();
                                if (input == "q")
                                {
                                    // do something
                                    bank.Menu();
                                    break;
                                }
                                else if (input == "y")
                                {
                                    // save new data
                                    Name = newUserData;
                                    break;
                                }
                                else if (input == "n")
                                {
                                }
                                else
                                {
                                    Console.WriteLine("wrong input");
                                }
                            }

                            break;
                        case "3":
                            while (true)
                            {
                                Console.WriteLine(" your new address is");
                                input = Console.ReadLine();
                                var newUserData = input;
                                Console.WriteLine($" your new address {newUserData}is ok? y/n or q");
                                input = Console.ReadLine();
                                if (input == "q")
                                {
                                    // do something
                                    bank.Menu();
                                    break;
                                }
                                else if (input == "y")
                                {
                                    // save new data
                                    SetAddress(newUserData);
                                    break;
                                }
                                else if (input == "n")
                                {
                                }
                                else
                                {
                                    Console.WriteLine("wrong input");
                                }
                            }

                            break;
                        case "4":
                            while (true)
                            {
                                Console.WriteLine(" your new passport is");
                                input = Console.ReadLine();
                                var newUserData = input;
                                Console.WriteLine($" your new passport{newUserData} is ok? y/n or q");
                                input = Console.ReadLine();
                                if (input == "q")
                                {
                                    // do something
                                    bank.Menu();
                                    break;
                                }
                                else if (input == "y")
                                {
                                    // save new data
                                    SetPassport(newUserData);
                                    break;
                                }
                                else if (input == "n")
                                {
                                }
                                else
                                {
                                    Console.WriteLine("wrong input");
                                }
                            }

                            break;
                        default:
                            Console.WriteLine("there is no such option");
                            break;
                    }
                }
                else if (input == "q")
                {
                    bank.Menu();
                    break;
                }
                else if (input == "n")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("wrong input");
                }
            }
        }
    }
}