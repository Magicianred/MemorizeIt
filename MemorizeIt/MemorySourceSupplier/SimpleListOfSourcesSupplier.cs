using System;
using System.Collections.Generic;

namespace MemorizeIt
{
	public class SimpleListOfSourcesSupplier:IListOfSourcesSupplier
	{
		private readonly IEnumerable<string> source;

		public SimpleListOfSourcesSupplier (IEnumerable<string> source)
		{
			this.source = source;
		}

		#region IListOfSourcesSupplier implementation

		public IEnumerable<string> GetSourcesList ()
		{
			return source;
		}

		#endregion
	}
}

