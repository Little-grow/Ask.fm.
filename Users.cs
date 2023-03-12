using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASK.fm
{
    internal class Users
    {
        static List<User> users = new List<User>();

        public void AddUser()
        {
            User user = new User();
            users.Add(user);
        }

        public static User? SearchUserById(int id)
        {
            for (int i = 0; i < users.Count ; i++)
            {
                if (users[i].Id == id)
                    return users[i];
            }
            return null;
        }

        public static void SignUp()
        {
            User user = new User();
            users.Add(user);
        }
    }
}
