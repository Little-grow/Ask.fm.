using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ASK.fm
{
    internal class AskSystem
    {
        static void LoadDatabase(bool fillUserQuestion = false)
        {
            UserManager.LoadDatabase();
            QuestionsManager.LoadDatabase();

            if (fillUserQuestion) //first time waiting for login
                QuestionsManager.FillUserQuestion(UserManager.currentUser);
        }

        // there's something wrong.
       static Dictionary<int, Action<User>> Choices = new Dictionary<int, Action<User>>()
       {
            { 1, user => QuestionsManager.PrintUserToQuestion(user)},
            { 2, user => QuestionsManager.PrintUserFromQuestion(user)},
            { 3, user => QuestionsManager.AnswerQuestion(user)},
            { 4, user => QuestionsManager.DeleteQuestion(user)},
            { 6, user => UserManager.ListUsersNamesIds()},
            { 7, user=>QuestionsManager.ListFeed() }

       };

        

       public static void Run()
       {
            LoadDatabase();
            UserManager.AccessSystem();
            QuestionsManager.FillUserQuestion(UserManager.currentUser);

            List<string> menu = new List<string> 
            { 
              "Print Questions To Me",
              "Print Questions From Me", 
              "Answer Question", 
              "Delete Question",
              "Ask Question",
              "List System Users", 
              "Feed", 
              "Logout"
            };
           

            while (true)
            {
                int choice = Helper.ShowReadMenu(menu);
                LoadDatabase(true);
                if (choice == -1)
                    break;
                else if (choice == 5)
                {
                    Tuple<int, bool> toUserPair = UserManager.ReadUserId();
                    if (toUserPair.Item1 != -1)
                    {
                        QuestionsManager.AskQuestion(UserManager.currentUser, toUserPair);
                        QuestionsManager.UpdateDatabase();
                    }
                }
                else
                    Choices[choice](UserManager.currentUser);
                if (choice == 4)
                    QuestionsManager.FillUserQuestion(UserManager.currentUser);
                if (choice == 3 || choice == 4)
                    QuestionsManager.UpdateDatabase();
            }
            Run();
        }
    }
}

