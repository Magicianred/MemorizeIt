using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemorizeIt.Model
{
    public class DictionaryTable
    {
        public DictionaryTable(string name, DictionaryItem[] items)
		{
			Name = name;
			Items = items;
		}
		public string Name{ get; private set;}
        public DictionaryItem[] Items { get;private set; }
    }
}
