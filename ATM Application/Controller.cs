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
            string userName, password,retypePassword;
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
            }while(retypePassword != password);

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
                if (user.userName == userName)
                {
                    return true;
                }
            }
            return false;
        }
        
        private bool registerToJson(string username, string password)
        {
            User user = new User { userName = username, password = password };
            string writeJson = JsonConvert.SerializeObject(user, Formatting.Indented);
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
                }else
                {
                    Console.WriteLine("Log in failed!");
                    Console.WriteLine("Do you want to retry your login?[y/n]");
                    
                }
            }
        }

        private bool validateLogIn(string userName, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            foreach (var user in users)
            {
                if (user.userName == userName && user.password == password)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
    
}