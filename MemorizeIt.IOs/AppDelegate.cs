using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MemorizeIt.IOs.Screens;
using FileMemoryStorage;

namespace MemorizeIt.IOs {
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate {
		// class-level declarations
		UIWindow window;
		UINavigationController navController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			// make the window visible
			window.MakeKeyAndVisible ();

			// create our nav controller
			navController = new UINavigationController ();
			var storage = new FileSystemMemoryStorage ();
			var tab1 = new HomeScreen(storage);
			var tab2 = new UpdateController(storage);

			var tabBarController = new UITabBarController ();
			tabBarController.ViewControllers = new UIViewController [] {
				tab1,
				tab2,
			};

			window.RootViewController = tabBarController;


			/*ViewControllers = tabs;
			tabController = new TabBarController ();*/

			
			// push the view controller onto the nav controller and show the window
		//	navController.PushViewController(tabController, false);
			//window.RootViewController = navController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}