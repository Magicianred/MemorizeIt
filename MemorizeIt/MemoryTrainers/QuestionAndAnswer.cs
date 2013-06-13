using System;

namespace MemorizeIt.MemoryTrainers
{
    public class QuestionAndAnswer
    {
        public Guid Id { get; private set; }
        public string Question { get; private set; }
        public string Answer { get; private set; }

        public QuestionAndAnswer(Guid id, string question, string answer)
        {
            Id = id;
            Question = question;
            Answer = answer;
        }
    }
}