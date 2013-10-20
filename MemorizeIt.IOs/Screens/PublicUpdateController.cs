using System;
using MemorizeIt.IOs.Screens;
using MemorizeIt.MemoryStorage;
using GoogleMemorySupplier;
using MemorizeIt.MemorySourceSupplier;
using MonoTouch.Dialog;

namespace MemorizeIt.IOs.Screens
{
	public class PublicUpdateController:GoogleUpdateController
	{
		public PublicUpdateController (IMemoryStorage store):base(store)
		{
		}
		
		protected override IMemoryFactory CreateSupplier (){
			return new PublicMemoryFactory ("0Av18WO5DffIrdF9zVzVHZHM2UzdqeDY0X1doSmxOZ3c", "memorize.it.test@gmail.com", "MemorizeIt");
		}
		protected override string GetSectionTitle ()
		{
			return "Public Memory Sources";
		}

		protected override string GetEmptyListReasonTitle ()
		{
			return "No sources at this time";
		}
	}
}

