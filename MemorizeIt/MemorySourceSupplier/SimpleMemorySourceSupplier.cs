using System;
using MemorizeIt.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MemorizeIt.MemorySourceSupplier
{
	public class SimpleMemorySourceSupplier:IMemorySourceSupplier
	{
		private readonly MemoryTable source;
		public SimpleMemorySourceSupplier (string name, IEnumerable<MemoryItem> items)
		{
			this.source = new MemoryTable(name,items.ToArray());
		}
		public MemoryTable Download(){
		//	throw new InvalidOperationException ("operation is not allowed");
			return source;
		}
	}
}

