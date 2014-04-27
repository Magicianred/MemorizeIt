using System;
using MemorizeIt.DictionarySourceSupplier;

namespace MemorizeIt
{
	public class TempDictionaryFactory: AbstractDictionaryFactory
	{

		protected override IDictionarySourceSupplier CreateMemorySourceSupplier()
		{
			return new TempDictionarySupplier ();
		}
	}

}

