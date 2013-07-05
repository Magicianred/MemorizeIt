using System;
using System.Collections.Generic;
using System.Linq;
using MemorizeIt.MemorySourceSupplier.CredentialsStorage;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.MemoryStorage;
using FileMemoryStorage;
using MemorizeIt.IOs.Screens;
using MonoTouch.SlideoutNavigation;

namespace MemorizeIt.IOs
{
	public partial class MenuController : DialogViewController
	{
		private readonly IMemoryStorage storage;
		private readonly ICredentialsStorage credentials;
		private readonly SlideoutNavigationController menu;

		private readonly UIViewController homeControlled;
		private readonly UIViewController googleUpdateController;

		public MenuController(SlideoutNavigationController menu,IMemoryStorage storage, ICredentialsStorage credentials) 
			: base(UITableViewStyle.Plain,new RootElement(""))
		{
			this.menu = menu;
			this.storage = storage;
			this.credentials = credentials;
			homeControlled = new HomeScreen (storage);
			googleUpdateController = new GoogleUpdateController (storage, credentials);
			menu.TopView = storage.Empty () ? googleUpdateController : homeControlled;
				
		}

		protected Element CreateMemoriesScreen(){
			var element = new StyledStringElement ("Memories", HandleMemoriesClick);
			return element;
		}

		protected void HandleMemoriesClick(){
			if (storage.Empty()) {
				new UIAlertView ("Memories are empty", "Please upload memories", null, "OK", null).Show ();

				return;
			}
			NavigationController.PushViewController (homeControlled, true);
		}

		protected Element CreateUpdateScreen(){
			return new StyledStringElement ("Update",
			                                () => {
				NavigationController.PushViewController (googleUpdateController, true);});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			var section = new Section ();
			section.Add (CreateMemoriesScreen());
			section.Add (CreateUpdateScreen());
		Root.Add (section);

		}
	}
}
