using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryStorage
{
    public interface IMemoryStorage
    {
        void Store(MemoryTable data);
        void Clear();
        
        MemoryItem Get(Guid id);
        IList<MemoryItem> Items { get; }

        void Success(Guid id);
        void Fail(Guid id);
        //MemoryItem MarkAsAccomplished();


        //get random
        //ask question
        //validate result
        //save result
    }
}
