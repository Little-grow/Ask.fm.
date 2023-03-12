using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASK.fm
{
    internal static class QuestionsManager
    {
        static Dictionary<int, List<int>> questionIdQuestionsThread = new Dictionary<int, List<int>>();
        static Dictionary<int, Question> questionIdQuestionObject = new Dictionary<int, Question>();
        static int lastId = 0; 
       

        public static void LoadDatabase()
       {
            questionIdQuestionsThread.Clear();
            questionIdQuestionObject.Clear();

            List<string> lines = Helper.ReadFileLines("Questions.txt");

            foreach (string line in lines)
            {
                var question = new Question(line);
                lastId = Math.Max(lastId, question.Id); // for generating ids

                questionIdQuestionObject[question.Id] = question; // base for the data

                if (question.ParentId == -1)
                    questionIdQuestionsThread[question.Id].Add(question.Id);
                else
                    questionIdQuestionsThread[question.ParentId].Add(question.Id);
            }
       }

        public static void FillUserQuestion( User user)
        {
            user.questionsIdFromMe.Clear();
            user.questionIdQuestionThread.Clear();

            foreach (var parent in questionIdQuestionsThread)
            {
                foreach (var id in parent.Value)
                {
                    Question question = questionIdQuestionObject[id];
                    if (question.FromUserId == user.Id)
                        user.questionsIdFromMe.Add(question.Id);
                    
                    if(question.ToUserId == user.Id)
                    {
                        if (question.ParentId == -1)
                            user.questionIdQuestionThread[question.Id].Add(question.Id);
                        else
                            user.questionIdQuestionThread[question.Id].Add(question.Id);
                    }
                }
            }
        }

        public static void PrintUserToQuestion( User user)
        {
            Console.WriteLine();
            if (user.questionIdQuestionThread.Count == 0)
            {
                Console.WriteLine("No Questions\n");
                return;
            }

            foreach (var pair in user.questionIdQuestionThread)
            {
                foreach (var id in pair.Value)
                {
                    Question question = questionIdQuestionObject[id];
                    question.PrintToQuestion();
                }
            }
            Console.WriteLine();
        }

        public static void PrintUserFromQuestion( User user)
        {
            Console.WriteLine();
            if (user.questionsIdFromMe.Count() == 0)
            {
                Console.WriteLine("No Questions");
                return;
            }

            foreach (var id in user.questionsIdFromMe)
            {
                Question question = questionIdQuestionObject[id]; 
                question.PrintFromQuestion();
            }
        }

        public static int ReadQuestionIdAny( User user)
        {
            Console.WriteLine("Enter question id or -1 to cancel: ");
            int id = int.Parse(Console.ReadLine());
            if (id == -1)
                return -1;

            if (!questionIdQuestionObject.ContainsKey(id))
            {
                Console.WriteLine("\nERROR: No question with such ID. Try again\n");
                return ReadQuestionIdAny(user);
            }

            Question question = questionIdQuestionObject[id];
            if(question.ToUserId != user.Id)
            {
                Console.WriteLine("\nERROR: Invalid question ID. Try again\n");
                return ReadQuestionIdAny(user);
            }
            return id;
        }

        public static int ReadQuestionIdThread(User user)
        {
            int id;
            Console.WriteLine("For thread qustion: Enter Question id or -1" +
                " for new question: ");

            id = int.Parse(Console.ReadLine());
            if (id == -1)
                return -1;
            if(!questionIdQuestionsThread.ContainsKey(id))
            {
                Console.WriteLine("No thread question with such ID. Try again \n");
                return ReadQuestionIdAny(user);
            }
            return id;
        }

        public static void AnswerQuestion(User user)
        {
            int id = ReadQuestionIdAny(user);
            if (id == -1)
                return;
            Question question = questionIdQuestionObject[id];
            question.PrintToQuestion();
            if(question.QuestionAnswer !="")
                Console.WriteLine("\nWarning: Already answered. Answer will be updated");

            Console.WriteLine("Enter answer:");
            question.QuestionAnswer = Console.ReadLine();
            question.QuestionAnswer += Console.ReadLine();
        }

        public static void DeleteQuestion(User user)
        {
            int id = ReadQuestionIdAny(user);
            if (id == -1)
                return;
            var idsToRemove = new List<int>();

            if(questionIdQuestionsThread.ContainsKey(id)) // thread
            {
                idsToRemove = questionIdQuestionsThread[id];
                questionIdQuestionsThread.Remove(id);
            }
            else
            {
                idsToRemove.Add(id);
                foreach (var item in questionIdQuestionsThread)
                {
                    List<int> list = item.Value;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (id == list[i])
                        {
                            list.Remove(id);
                            break;
                        }
                    }
                }
            }

            foreach (var _id in idsToRemove)
            {
                questionIdQuestionsThread.Remove(_id);
            }
        }

        public static void AskQuestion(User user , Tuple<int,bool> toUserPair)
        {
            Question question = new Question();

            if(!toUserPair.Item2)
            {
                Console.WriteLine("Note: Anonymous questions are not allowed for this user\n");
                question.IsAnonQuestion = false; 
            }
            else
            {
                Console.WriteLine("Is anonympus question?: (0 or 1):");
                question.IsAnonQuestion = int.Parse(Console.ReadLine()) == 1;
            }

            question.ParentId = ReadQuestionIdThread(user);

            Console.WriteLine("Enter Question text: ");
            question.QuestionText = Console.ReadLine();

            question.FromUserId = user.Id;
            question.ToUserId = toUserPair.Item1;

            question.Id = ++lastId;

            questionIdQuestionObject[question.Id] = question;

            if (question.ParentId == -1)
                questionIdQuestionsThread[question.Id].Add(question.Id);
            else
                questionIdQuestionsThread[question.ParentId].Add(question.Id);
        } // Shahd, Reread this method again

        public static void ListFeed()
        {
            foreach (var pair in questionIdQuestionObject)
            {
                Question question = pair.Value;
                if (question.QuestionAnswer == "")
                    continue;
                question.PrintFeedQuestion();
            }
        }

        public static void UpdateDatabase()
        {
            var lines = new List<string>();
            foreach (var pair in questionIdQuestionObject)
            {
                lines.Add(pair.Value.ToString());
            }
            Helper.WriteFileLines("questions.txt", lines, false);
        }
    }
}
