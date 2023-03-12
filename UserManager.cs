using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ASK.fm
{
    internal static class UserManager
    {
        static Dictionary<string, User> userNameUserObject = new Dictionary<string, User>();
        public static User currentUser = new User();
        static int lastId = 0;

      
        public static void LoadDatabase()
        {
            lastId = 0;
            userNameUserObject.Clear();

            List<string> lines = Helper.ReadFileLines("users.txt");
            foreach (var line in lines)
            {
                if (line != "")
                {
                    User user = new User(line);
                    userNameUserObject[user.UserName] = user;
                    lastId = Math.Max(lastId, user.Id);
                }
            }
        }

        public static void AccessSystem()
        {
            int choice = Helper.ShowReadMenu(new List<string> { "Login", "Sign Up" });
            if (choice == 1)
                DoLogin();
            else
                DoSignUp();
        }

        public static void DoLogin()
        {
            LoadDatabase();
            while (true)
            {
                Console.WriteLine("Enter user name");
                currentUser.UserName = Console.ReadLine();

                Console.WriteLine("Enter password");
                currentUser.Password = Console.ReadLine();

                if(!userNameUserObject.ContainsKey(currentUser.UserName))
                {
                    Console.WriteLine("\nInvalid user name or password. Try again\n");
                    continue;
                }

                User user = userNameUserObject[currentUser.UserName];
                currentUser = user;
                break;
            }
        }

        public static void DoSignUp()
        {
            while (true)
            {
                Console.WriteLine("Enter user name. (No spaces): ");
                currentUser.UserName = Console.ReadLine();

                if (userNameUserObject.ContainsKey(currentUser.UserName))
                    Console.WriteLine("Already used. Try again");
                else
                    break;
            }
            Console.Write("Enter password: ");
            currentUser.Password = Console.ReadLine();

            Console.Write("Enter name: ");
            currentUser.Name = Console.ReadLine();

            Console.Write("Enter email: ");
            currentUser.Email = Console.ReadLine();

            Console.Write("Allow anonymous questions? (0 or 1): ");
            bool allowAnonymousQuestions = Convert.ToBoolean(Convert.ToInt32(Console.ReadLine()));
            currentUser.AllowAnonymousQuestion = allowAnonymousQuestions;

            currentUser.Id = ++lastId;
            userNameUserObject[currentUser.UserName] = currentUser;

            UpdateDatabase(currentUser);
        }

        public static void ListUsersNamesIds()
        {
            foreach (var pair in userNameUserObject)
            {
                Console.WriteLine($"Id {pair.Value.Id}\t\tName: {pair.Value.Name}");
            }
        }

        public static Tuple<int, bool> ReadUserId()
        {
            Console.WriteLine("Enter user id or -1 to cnacel: ");
            int id = int.Parse(Console.ReadLine());

            if (id == -1)
            {
                return new Tuple<int, bool>(-1, false);
            }

            foreach (var item in userNameUserObject)
            {
                if (item.Value.Id == id)
                    return new Tuple<int, bool>(id, item.Value.AllowAnonymousQuestion);
            }
            Console.WriteLine("Invalid User Id. Try again");
            return ReadUserId();
        }

        public static void UpdateDatabase(User user)
        {
            string line = user.ToString();
            List<string> lines = new List<string> { line };
            Helper.WriteFileLines("users.txt", lines);
        }
    }
}
