using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks.UI
{
    public class UIManager
    {
        private static UIManager _instance;

        private List<string> menuItems = new List<string>()
        {
            "FindBanks",
            "q",
        };
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }

                return _instance;
            }
        }

        public void ShowAccountMenu(AccountUI accountUI)
        {
            accountUI.Menu();
        }

        public void ShowClientMenu(ClientUI clientUI, Bank bank)
        {
            clientUI.Menu(bank);
        }

        public void ShowBankMenu(UIBank uiBank)
        {
            uiBank.Menu();
        }

        public void MainMenu()
        {
            Console.Clear();
            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(menuItem);
            }

            while (true)
            {
                string selectedMenuItem = Console.ReadLine();
                if (selectedMenuItem == menuItems[0])
                {
                    BanksMenu();
                }
                else if (selectedMenuItem == menuItems[1])
                {
                    Environment.Exit(0); // Or  System.Environment.Exit(0);
                }
            }
        }

        public void BanksMenu()
        {
            // get all possible banks to show the user
            if (CentralBank.Instance == null)
            {
                throw new Exception("ERROR in BanksMenu: There is no Central Bank");
            }

            List<Bank> banks = CentralBank.Instance.Banks;
            foreach (var bank in banks)
            {
                Console.WriteLine(bank.Name);
            }

            while (true)
            {
                Console.WriteLine("Choose bank(name) or press q to quit");
                string selectedBank = Console.ReadLine();
                if (selectedBank == "q") MainMenu();
                var bank = banks.FirstOrDefault(t => t.Name == selectedBank);
                if (bank != null)
                {
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowBankMenu(new UIBank(bank));
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("There is no such bank");
                }
            }
        }
    }
}