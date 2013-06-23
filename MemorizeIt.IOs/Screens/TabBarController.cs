using System;
using MonoTouch.UIKit;

namespace MemorizeIt.IOs.Screens
{
	public class TabBarController : UITabBarController {

		UIViewController tab1, tab2;

		public TabBarController ()
		{

			tab1 = new HomeScreen();
			tab2 = new UpdateController();
			tab2.Title = "Update";

			var tabs = new UIViewController[] {
				tab1, tab2
			};

			ViewControllers = tabs;
		}
	}
}

