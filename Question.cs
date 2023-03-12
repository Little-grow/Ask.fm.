using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASK.fm
{
    internal class Question
    {

        public int Id { get; set; }
        public int ParentId { get; set; }

        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        public bool IsAnonQuestion { get; set; }

        public string QuestionText { get; set; }
        public string QuestionAnswer { get; set; }



        public Question()
        {
            Id = ParentId = FromUserId = ToUserId = -1;
            IsAnonQuestion = true;
            QuestionText = QuestionAnswer = "";
        }

        public Question(string question)
        {
            string[] elements = question.Split(',');

            Id = int.Parse(elements[0]);
            ParentId = int.Parse(elements[1]);

            FromUserId = int.Parse(elements[2]);
            ToUserId = int.Parse(elements[3]);

            IsAnonQuestion = bool.Parse(elements[4]);

            QuestionText = elements[5];
            QuestionAnswer = elements[6];
        }

        public override string ToString()
        {
            string que = string.Format("{0},{1},{2},{3},{4},{5},{6}", Id, ParentId, FromUserId, ToUserId,
                IsAnonQuestion, QuestionText, QuestionAnswer);
            return que;
        }
        
        public void PrintToQuestion()
        {
            string prefix = "";
            if (ParentId != -1)
                prefix = "Thread: ";

            Console.Write($"{prefix} Question Id ({Id})");

            if(!IsAnonQuestion)
                Console.Write($" from user id{FromUserId}");

            Console.WriteLine($"\t Question: {QuestionText}");

            if (QuestionAnswer != "")
                Console.WriteLine($"\t Answer: {QuestionAnswer}");
        }

        public void PrintFromQuestion()
        {
            string anon = "";
            if (IsAnonQuestion)
                anon = "!AQ";

            Console.Write($"Question Id {anon} to user id ({ToUserId}) ");
            Console.Write($"\t Question: {QuestionText}");

            if (QuestionAnswer == "")
                Console.WriteLine(" not answered yet");
            else
                Console.WriteLine($"\t Answer: {QuestionAnswer}");
        }

        public void PrintFeedQuestion()
        {
            string prefix = "";
            if (ParentId == -1)
                prefix = $"Thread parent question Id ({ParentId}) ";
            Console.Write($"{prefix}question id ({Id})");

            string From = "";
            if (!IsAnonQuestion)
                From = $"from user id ({FromUserId})";

            Console.Write($" {prefix} to user id {ToUserId}");
            Console.WriteLine($"\t{QuestionText}");

            if (QuestionAnswer != "")
                Console.WriteLine($"\tAnswer: {QuestionAnswer}");
        }
    }
}
