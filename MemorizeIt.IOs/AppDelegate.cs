using System;
using System.Collections.Generic;
using System.Linq;
using GoogleDictionarySupplier;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MemorizeIt.IOs.Screens;
using FileDictionaryStorage;
using MonoTouch.SlideoutNavigation;
using MemorizeIt.DictionaryStorage;

namespace MemorizeIt.IOs {
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate {
		// class-level declarations
		UIWindow window;

	//	public SlideoutNavigationController Menu { get; private set; }
		public IDictionaryStorage storage{ get;private set;}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			storage = new FileSystemDictionaryStorage ();
		/*	Menu = new CustomSlideoutNavigationController ();

		    Menu.MenuView = new MenuController(Menu, storage);*/
			
			var navigation = new UINavigationController ();
			var homeScreen = new HomeScreen (storage);
			navigation.PushViewController (homeScreen, true);		
			window.RootViewController = navigation;
	//		window.RootViewController = Menu;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}