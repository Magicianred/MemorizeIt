using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemorizeIt.Model
{
    public class MemoryTable
    {
        public MemoryTable(string name, MemoryItem[] items)
		{
			Name = name;
			Items = items;
		}
		public string Name{ get; private set;}
        public MemoryItem[] Items { get;private set; }
    }
}
