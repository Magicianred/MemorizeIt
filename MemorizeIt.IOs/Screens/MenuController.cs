using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly  Dictionary<string, UIViewController> controllers;
		public MenuController(SlideoutNavigationController menu,IMemoryStorage storage, ICredentialsStorage credentials) 
			: base(UITableViewStyle.Plain,new RootElement(""))
		{
			this.menu = menu;
			this.storage = storage;
			this.credentials = credentials;
			controllers = new Dictionary<string, UIViewController> ();
			controllers.Add ("Memories", new HomeScreen (storage));
			controllers.Add ("Update", new GoogleUpdateController (storage, credentials));
			menu.TopView = storage.Empty() ? controllers["Update"] : controllers["Memories"];
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Root.Add(new Section() {
				controllers.Select(c=> (Element)new StyledStringElement(c.Key,()=>{NavigationController.PushViewController(c.Value,true);}))
				/*new StyledStringElement("Memories", () => { NavigationController.PushViewController(new HomeScreen(storage), true); }),
				new StyledStringElement("Update", () => { NavigationController.PushViewController(new GoogleUpdateController(storage, credentials), true); })
*/
			});

		}
	}
}
