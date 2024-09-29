using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ATM_Application
{
    public class Controller
    {
        private string currentUser; // To store the username of the current user

        public void run()
        {
            Console.WriteLine("=======Atm Application=======");
            MenuLogOrReg();
        }

/**
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

/**
 * Method that let the user select his/her choice
 */
        private void GetChoiceLogOrReg()
        {
            try
            {
                while (true)
                {
                    int choice;
                    Console.Write("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            LogIn();
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
/**
 * Method which will allow the user to open an account
 */
        private void OpenAccount()
        {
            string userName, password, retypePassword;
            Console.Clear();
            do
            {
                Console.WriteLine("Enter your username: ");
                userName = Console.ReadLine();
                if (IsUserNameTaken(userName))
                {
                    Console.WriteLine("Username is already taken!");
                }
                else
                {
                    break;
                }

            } while (true);

            Console.Write("Enter your password: ");
            password = Console.ReadLine();
            do
            {
                Console.Write("Enter your retype password: ");
                retypePassword = Console.ReadLine();
                if (retypePassword != password)
                {
                    Console.WriteLine("Password Unmatched! please try again");
                }
            } while (retypePassword != password);

            if (RegisterToJson(userName, password))
            {
                Console.WriteLine("Account successfully registered!");
            }
            else
            {
                Console.WriteLine("Error on registration!");
            }
        }
/**
 * Check the json file if the username is already taken
 */
        private bool IsUserNameTaken(string userName)
        {
            string json = File.ReadAllText("users.json");
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
/**
 * Add the user in the Json file 
 */
        private bool RegisterToJson(string username, string password)
        {
            string json = File.ReadAllText("users.json");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            User newUser = new User { UserName = username, Password = password };
            users.Add(newUser);
            string writeJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("users.json", writeJson);
            return true;
        }
/**
 * Method that will handle the user login
 */
        private void LogIn()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter your username: ");
                var userName = Console.ReadLine();
                Console.Write("Enter your password: ");
                var password = Console.ReadLine();
                if (ValidateLogIn(userName, password))
                {
                    Console.WriteLine("Log in successful!");
                    currentUser = userName;
                    AtmMenu();
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
/**
 * Method that will check or validate user login in the Json file
 */
        private bool ValidateLogIn(string userName, string password)
        {
            string json = File.ReadAllText("users.json");
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

        /**
         * Method that will display the menu of the program
         */
        private void AtmMenu()
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
        /**
         * Method that will display the menu of the program
         */
        private void choiceMenu()
        {
            try
            {
                Console.Write("Enter your choice: ");
                var choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        CheckBalance();
                        break;
                    case 2:
                        WithdrawCash();
                        break;
                    case 3:
                        DepositCash();
                        break;
                    case 4:
                        currentUser = "";
                        MenuLogOrReg();
                        break;
                    default:
                        Console.WriteLine("Invalid input! please try again...");
                        break;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid input! please try again...");
            }
        }
        
        /**
         * Method that will check the account balance of the user in the JSON file
         */
        private void CheckBalance()
        {
            string json = File.ReadAllText("users.json");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            foreach (var user in users)
            {
                if (user.UserName == currentUser)
                {
                    Console.WriteLine("Your total balance: " + user.Amount);
                    Console.WriteLine("Press enter to continue...");
                    string input = Console.ReadLine();
                }
            }
        }
        /**
         * Method that will handle the deposit process of the user
         */
        private void DepositCash()
        {
            Console.Write("Enter the amount you want to deposit: ");
            var amount = double.Parse(Console.ReadLine());
            if (!DepositJson(amount))
            {
                Console.WriteLine("Problem occured!");
            }
            Console.WriteLine("Press enter to continue...");
            string input = Console.ReadLine();
        }
        /**
         * Method that will update the deposit transaction of the user in the JSON file
         */
        private bool DepositJson(double amount)
        {
            string json = File.ReadAllText("users.json");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            User updateAmount = users.FirstOrDefault(user => user.UserName == currentUser);
            if (updateAmount != null)
            {
                updateAmount.Amount += amount;
                string updatedJson = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText("users.json", updatedJson);
                Console.WriteLine("Succesfully deposited!");
                Console.WriteLine("Your total balance: " +  updateAmount.Amount);
                return true;
            }

            return false;
        }
        /**
         * Method that will handle the cash withdrawal process of the program
         */
        private void WithdrawCash()
        {
            Console.Write("Enter the amount you want to withdraw: ");
            var amount = double.Parse(Console.ReadLine());
            if (!WithdrawJson(amount))
            {
                Console.WriteLine("Insufficient balance!");
            }
            Console.WriteLine("Press enter to continue...");
            string input = Console.ReadLine();
        }
    /**
     * Method that will update the JSON file in the cash withdrawal process
     */
        private bool WithdrawJson(double amount)
        {
            string json = File.ReadAllText("users.json");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            User updateAmount = users.FirstOrDefault(user => user.UserName == currentUser);
            if (updateAmount != null)
            {
                if (updateAmount.Amount < amount)
                {
                    return false;
                }
                updateAmount.Amount -= amount;
                string updatedJson = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText("users.json", updatedJson);
                Console.WriteLine("Succesfully withdrawn!");
                Console.WriteLine("Your total balance: " + updateAmount.Amount);
                return true;
            }
            return false;
        }
    }
}
