using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.DictionaryStorage;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryTrainers
{
    public class SimpleRandomizer : IRandomizer
    {
        private readonly Random random;
        private readonly IDictionaryStorage store;
        public SimpleRandomizer(IDictionaryStorage store)
        {
            this.random=new Random();
            this.store = store;
        }

        public QuestionAndAnswer GetRandomUnsuccessItem()
        {
			var items = store.Items.Where (i=>!i.IsAccomplished).ToList();
			if (items.Count == 0)
				return null;
            var ind = random.Next(0, items.Count);
            var item = items[ind];

            var questionIndex = random.Next(0, 2);
            return new QuestionAndAnswer(item.Id, item.Values[questionIndex], item.Values[1 - questionIndex]);
        }
    }
}
