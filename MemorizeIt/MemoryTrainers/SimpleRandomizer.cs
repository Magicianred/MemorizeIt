using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.MemoryStorage;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryTrainers
{
    public class SimpleRandomizer : IRandomizer
    {
        private readonly Random random;
        private readonly IMemoryStorage store;
        public SimpleRandomizer(IMemoryStorage store)
        {
            this.random=new Random();
            this.store = store;
        }

        public QuestionAndAnswer GetRandomUnsuccessItem()
        {
            var items = store.Items;
            var ind = random.Next(0, items.Count - 1);
            var item = items[ind];

            var questionIndex = random.Next(0, 2);
            return new QuestionAndAnswer(item.Id, item.Values[questionIndex], item.Values[1 - questionIndex]);
        }
    }
}
