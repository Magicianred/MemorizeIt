using System;
using MonoTouch.SlideoutNavigation;

namespace MemorizeIt.IOs
{
	public class CustomSlideoutNavigationController:SlideoutNavigationController
	{
		public CustomSlideoutNavigationController ()
		{
		}
		protected override MonoTouch.UIKit.UIBarButtonItem CreateMenuButton ()
		{
			var button= base.CreateMenuButton ();
			button.Title="...";
			return button;
		}
	}
}

