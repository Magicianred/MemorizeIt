using System;
using MonoTouch.SlideoutNavigation;
using MonoTouch.UIKit;

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
			button.Image = UIImage.FromFile ("menu-list.png");
			button.Title="";
			return button;
		}
	}
}

