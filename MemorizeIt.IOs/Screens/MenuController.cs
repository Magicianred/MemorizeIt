using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.MemoryStorage;
using FileMemoryStorage;
using MemorizeIt.IOs.Screens;

namespace MemorizeIt.IOs
{
	public partial class MenuController : DialogViewController
	{
		private readonly IMemoryStorage storage;
		
		private readonly ICredentialsStorage credentials;

		public MenuController(IMemoryStorage storage, ICredentialsStorage credentials) 
			: base(UITableViewStyle.Plain,new RootElement(""))
		{
			this.storage = storage;
			this.credentials = credentials;

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Root.Add(new Section() {
				new StyledStringElement("Memories", () => { NavigationController.PushViewController(new HomeScreen(storage), true); }),
				new StyledStringElement("Update", () => { NavigationController.PushViewController(new GoogleUpdateController(storage, credentials), true); })

			});
		}
	}
}
