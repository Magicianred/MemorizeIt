using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MemorizeIt.Model
{
    public class MemoryItem
    {
        public MemoryItem(string[] values)
        {
            Id = Guid.NewGuid();
            Values = values;
            SuccessCount = 0;
        }

        [JsonConstructor]
        public MemoryItem(Guid id, string[] values, int successCount)
        {
            Id = id;
            Values = values;
            SuccessCount = successCount;
        }

        public Guid Id { get;private set; }

        public string[] Values { get; private set; }
        

        public bool IsAccomplished {
            get { return SuccessCount < SuccessCountForAccomplish; }
        }
        public int SuccessCount { get; private set; }

        public void Upvote()
        {
            this.SuccessCount++;
        }

        public void Downvote()
        {
            this.SuccessCount--;
        }

        private const int SuccessCountForAccomplish = 20;
    }
}
