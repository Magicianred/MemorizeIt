using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.MemoryStorage;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryTrainers
{
    public class SimpleMemoryTrainer
    {
        private readonly IMemoryStorage storage;
        private readonly IRandomizer randomizer;

        public SimpleMemoryTrainer(IMemoryStorage storage, IRandomizer randomizer)
        {
            this.storage = storage;
            this.randomizer = randomizer;
        }

        public string GetQuestion()
        {
            if (currentQuestionAndAnswer != null)
                throw new InvalidOperationException("question is not empty, can't start new one");
            
            currentQuestionAndAnswer = this.randomizer.GetRandomUnsuccessItem();
            return currentQuestionAndAnswer.Question;
        }

        public bool Validate(string answer)
        {
            if(currentQuestionAndAnswer==null)
                throw new InvalidOperationException("question is empty");
            var result = Validate(answer, currentQuestionAndAnswer.Answer);
            if (result)
                this.storage.Success(currentQuestionAndAnswer.Id);
            else
                this.storage.Fail(currentQuestionAndAnswer.Id);
            return result;
        }

        private bool Validate(string answer, string correctAnswer)
        {
            return
                string.Compare(answer.Trim(), correctAnswer.Trim(), StringComparison.CurrentCultureIgnoreCase) ==
                0;
        }


        private QuestionAndAnswer currentQuestionAndAnswer;
    }
}
