using System;
using System.Collections.Generic;

namespace MemorizeIt
{
	public interface IListOfSourcesSupplier
	{
		IEnumerable<string> GetSourcesList();
	}
}

