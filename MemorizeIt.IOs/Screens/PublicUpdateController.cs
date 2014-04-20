using System;
using MemorizeIt.IOs.Screens;
using MemorizeIt.DictionaryStorage;
using GoogleDictionarySupplier;
using MemorizeIt.DictionarySourceSupplier;
using MonoTouch.Dialog;

namespace MemorizeIt.IOs.Screens
{
	public class PublicUpdateController:GoogleUpdateController
	{
		public PublicUpdateController (IDictionaryStorage store):base(store)
		{
		}
		
		protected override IDictionaryFactory CreateSupplier (){
			return new PublicDictionaryFactory ("0Av18WO5DffIrdF9zVzVHZHM2UzdqeDY0X1doSmxOZ3c", "memorize.it.test@gmail.com", "MemorizeIt");
		}
		protected override string GetSectionTitle ()
		{
			return "Public dictionaries";
		}

		protected override string GetEmptyListReasonTitle ()
		{
			return "No dictionaries at this time";
		}
	}
}

