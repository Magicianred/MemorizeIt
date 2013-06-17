using System;
using MemorizeIt.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MemorizeIt.MemorySourceSupplier
{
	public class SimpleMemorySourceSupplier:IMemorySourceSupplier
	{
		private readonly IEnumerable<MemoryItem> source;
		public SimpleMemorySourceSupplier (IEnumerable<MemoryItem> source)
		{
			this.source = source;
		}
		public MemoryTable Download(){
		//	throw new InvalidOperationException ("operation is not allowed");
			return new MemoryTable (source.ToArray());
		}
	}
}

