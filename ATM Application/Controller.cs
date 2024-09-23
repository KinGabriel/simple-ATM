using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ATM_Application
{
    public class Controller
    {
        private string json = File.ReadAllText("users.json");
        private string currentUser;

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
                            logIn();
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
            string userName, password, retypePassword;
            Console.Clear();
            do
            {
                Console.WriteLine("Enter your username: ");
                userName = Console.ReadLine();
                if (isUserNameTaken(userName))
                {
                    Console.WriteLine("Username is already taken!");
                }
                else
                {
                    break;
                }

            } while (true);

            Console.WriteLine("Enter your password: ");
            password = Console.ReadLine();
            do
            {
                Console.WriteLine("Enter your retype password: ");
                retypePassword = Console.ReadLine();
                if (retypePassword != password)
                {
                    Console.WriteLine("Password Unmatched! please try again");
                }
            } while (retypePassword != password);

            if (registerToJson(userName, password))
            {
                Console.WriteLine("Account successfully registered!");
            }
            else
            {
                Console.WriteLine("Error on registration!");
            }
        }

        private bool isUserNameTaken(string userName)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            foreach (var user in users)
            {
                if (user.UserName == userName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool registerToJson(string username, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            User newUser = new User { UserName = username, Password = password };
            users.Add(newUser);
            string writeJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("users.json", writeJson);
            return true;
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
                    currentUser = userName;
                    atmMenu();
                }
                else
                {
                    Console.WriteLine("Log in failed!");
                    while (true)
                    {
                        Console.WriteLine("Do you want to retry your login?[y/n]");
                        char choice = Console.ReadKey().KeyChar;
                        if (choice == 'y' || choice == 'Y')
                        {
                            break;
                        }
                        else if (choice == 'n' || choice == 'N')
                        {
                            MenuLogOrReg();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input!");
                        }
                    }


                }
            }
        }

        private bool validateLogIn(string userName, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            foreach (var user in users)
            {
                if (user.UserName == userName && user.Password == password)
                {
                    return true;
                }
            }

            return false;
        }


        private void atmMenu()
        {
            while (true)
            {
                Console.WriteLine("====================");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Withdraw cash");
                Console.WriteLine("3. Deposit Cash");
                Console.WriteLine("4. Exit");
                choiceMenu();
            }
        }

        private void choiceMenu()
        {
            int choice;
            Console.WriteLine("Enter your choice: ");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    checkBalance();
                    break;
                case 2:
                    break;
                case 3:
                    depositCash();
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }

        public void checkBalance()
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            foreach (var user in users)
            {
                if (user.UserName == currentUser)
                    Console.WriteLine("Your total balance: " + user.Amount);
                    Console.WriteLine("Press enter to continue...");
                    string input = Console.ReadLine();
            }
        }

        public void depositCash()
        {
            double amount;
            Console.WriteLine("Enter the amount you want to deposit: ");
            amount = double.Parse(Console.ReadLine()); 
            if (depostiJSON(amount))
                    {
                        Console.WriteLine("Succesfully deposited!");
                        Console.WriteLine("Your total balance: " + amount);
                    }
                    else
                    {
                        Console.WriteLine("Problem occured!");
                   }
        }
        
        public bool depostiJSON(double amount)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            User updateAmount = users.FirstOrDefault(user => user.UserName == currentUser);
            if (updateAmount != null)
            {
                updateAmount.Amount += amount;
                string updatedJson = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText("users.json", updatedJson);
                return true; 
            }
            return false; 
        }
    }
}