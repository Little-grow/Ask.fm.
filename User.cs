using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ASK.fm
{
    internal class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool AllowAnonymousQuestion { get; set; }
        readonly Tuple<int, List<int>> questionIdQuestionsThread; // thread questions for this user 

        public bool AQ { get; set; }

        public List<int> questionsIdFromMe;
        public Dictionary<int, List<int>> questionIdQuestionThread;

        public User()
        {
            Id = -1;
        }

        public User(string line)
        {
            string[] elements = line.Split(',');
            Id = int.Parse(elements[0]);
            UserName = elements[1];
            Password = elements[2];
            Name = elements[3];
            Email = elements[4];
            AllowAnonymousQuestion = bool.Parse(elements[5]);
        }

        public override string ToString()
        {
            string user = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", Id, UserName, Password, Name, Email, AllowAnonymousQuestion);
            return user;
        }

        void print()
        {
            Console.WriteLine($"User {Id}, {UserName}, {Password}, {Name}, {Email}");
        }
    }
}

