using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MemorizeIt.IOs.Screens;
using FileMemoryStorage;
using MonoTouch.SlideoutNavigation;
using MemorizeIt.MemoryStorage;

namespace MemorizeIt.IOs {
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate {
		// class-level declarations
		UIWindow window;

		public SlideoutNavigationController Menu { get; private set; }
		public IMemoryStorage storage{ get;private set;}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			storage = new FileSystemMemoryStorage ();
			Menu = new SlideoutNavigationController();

			Menu.TopView = new HomeScreen(storage);
			Menu.MenuView = new MenuController (storage);

			window.RootViewController = Menu;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}