using System;

namespace Banks.UI
{
    public class ClientUI
    {
        private Client _client;

        public ClientUI(Client client)
        {
            _client = client;
        }

        public void Menu(Bank bank)
        {
            Console.WriteLine("welcome to client menu");
            Console.WriteLine("1) your surname: " + _client.Surname);
            Console.WriteLine("2) your name: " + _client.Name);
            Console.WriteLine("3) your address: " + _client.Address);
            Console.WriteLine("4) your passport: " + _client.Passport);
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
                                    if (UIManager.Instance != null)
                                    {
                                        UIManager.Instance.ShowBankMenu(new UIBank(bank));
                                    }

                                    break;
                                }

                                if (input != string.Empty)
                                {
                                    if (input == "y")
                                    {
                                        // save new data
                                        _client.Surname = newUserData;
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
                                else
                                {
                                    Console.WriteLine("surname is empty, enter surname please");
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
                                    if (UIManager.Instance != null)
                                    {
                                        UIManager.Instance.ShowBankMenu(new UIBank(bank));
                                    }

                                    break;
                                }

                                if (input != string.Empty)
                                {
                                    if (input == "y")
                                    {
                                        // save new data
                                        _client.Name = newUserData;
                                        break;
                                    }
                                    else if (input == "n")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("wrong input");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("name is empty, enter name please");
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
                                    if (UIManager.Instance != null)
                                    {
                                        UIManager.Instance.ShowBankMenu(new UIBank(bank));
                                    }

                                    break;
                                }

                                if (newUserData != string.Empty)
                                {
                                    if (input == "y")
                                    {
                                        // save new data
                                        _client.SetAddress(newUserData);
                                        break;
                                    }
                                    else if (input == "n")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("wrong input");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("address is empty, enter address please");
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
                                    if (UIManager.Instance != null)
                                    {
                                        UIManager.Instance.ShowBankMenu(new UIBank(bank));
                                    }

                                    break;
                                }

                                if (newUserData != string.Empty)
                                {
                                    if (input == "y")
                                    {
                                        // save new data
                                        _client.SetPassport(newUserData);
                                        break;
                                    }
                                    else if (input == "n")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("wrong input");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("passport is empty, enter passport please");
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
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowBankMenu(new UIBank(bank));
                    }

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