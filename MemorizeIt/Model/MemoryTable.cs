using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemorizeIt.Model
{
    public class MemoryTable
    {
        public MemoryTable(MemoryItem[] items)
        {
            Items = items;
        }

        public MemoryItem[] Items { get;private set; }
    }
}
