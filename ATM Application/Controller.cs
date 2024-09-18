using System;
using System.IO;

namespace ATM_Application
{
    public class Controller
    {
        public void run()
        {
            MenuLogOrReg();
           
        }
/*
 * Menu for the login and register
 */
            private void MenuLogOrReg()
        {
            while (true)
            {
                Console.WriteLine("1. Log in your account");
                Console.WriteLine("2. Open an account");
                Console.WriteLine("3. Exit");
                GetChoiceLogOrReg();
            }
        }
/*
 * Method that let the user select his/her choice
 */
        private void GetChoiceLogOrReg()
        {
            try
            {
                while (true)
                {
                    int choice;
                    Console.WriteLine("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            break;
                        case 2:
                            OpenAccount();
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Invalid choice! the allowed inputs are: 1-3");
                            break;
                    }
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid choice! the allowed inputs are: 1-3");
            }
        }

        private void OpenAccount()
        {
            string userName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Enter your username: ");
            
        }

        private void logIn()
        {
            while (true)
            {
                string userName;
                string password;
                Console.Clear();
                Console.WriteLine("Enter your username: ");
                userName = Console.ReadLine();
                Console.WriteLine("Enter your password: ");
                password = Console.ReadLine();
                if (validateLogIn(userName, password))
                {
                    Console.WriteLine("Log in successful!");
                }else
                {
                    Console.WriteLine("Log in failed!");
                    Console.WriteLine("Do you want to retry your login?[y/n]");
                    
                }
            }
        }

        private bool validateLogIn(string userName, string password)
        {
            string[] readCSV = File.ReadAllLines("../../../CSV/users.csv");
            foreach (string line in readCSV)
            {
                string[] splitLine = line.Split(',');
                if (splitLine[0] == userName && splitLine[1] == password)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
    
}