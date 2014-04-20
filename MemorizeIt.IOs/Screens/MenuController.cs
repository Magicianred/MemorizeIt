using System;
using System.Collections.Generic;
using System.Linq;
using MemorizeIt.DictionarySourceSupplier.CredentialsStorage;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.DictionaryStorage;

using MemorizeIt.IOs.Screens;
using MonoTouch.SlideoutNavigation;

namespace MemorizeIt.IOs
{
	public partial class MenuController : DialogViewController
	{
		private readonly IDictionaryStorage storage;
		private readonly SlideoutNavigationController menu;

		private readonly UIViewController homeControlled;
		private readonly UIViewController updateController;

		public MenuController(SlideoutNavigationController menu,IDictionaryStorage storage) 
			: base(UITableViewStyle.Plain,new RootElement(""))
		{
			this.menu = menu;
			this.storage = storage;
			homeControlled = new HomeScreen (storage);
			updateController = new SourceTypeController (storage);
			menu.TopView = storage.Empty () ? updateController : homeControlled;
				
		}

		public void ShowHomeController(){
			menu.TopView = homeControlled;
		}

		protected Element CreateMemoriesScreen(){
			var element = new StyledStringElement ("Memories", HandleMemoriesClick);
			return element;
		}

		protected void HandleMemoriesClick(){
			if (storage.Empty()) {
				new UIAlertView ("Nothing to learn", "Please load from sources", null, "OK", null).Show ();

				return;
			}
			NavigationController.PushViewController (homeControlled, true);
		}

		protected Element CreateUpdateScreen(){
			return new StyledStringElement ("Update",
			                                () => {
				NavigationController.PushViewController (updateController, true);});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			var section = new Section ();
			section.Add (CreateMemoriesScreen ());
			section.Add (CreateUpdateScreen ());
			Root.Add (section);

		}
	}
}
