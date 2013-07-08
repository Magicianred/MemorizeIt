using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MemorizeIt.MemoryStorage;


namespace MemorizeIt.IOs.Screens
{
	public partial class SourceTypeController : DialogViewController
	{
		private  readonly DialogViewController privateController;
		private  UIViewController publicController;
		private readonly IMemoryStorage store;


		public SourceTypeController (IMemoryStorage store) : base (UITableViewStyle.Grouped, null)
		{
			this.store = store;
			privateController = new GoogleUpdateController (store);

			Root = new RootElement ("SourceTypeController") {
				new Section (""){
					new StringElement ("Public", () => {
						NavigationController.PushViewController (publicController, true);
					})
				},
				new Section ("Private"){ new RootElement("Private",(r)=>privateController)}

			};
		}
	}
}
