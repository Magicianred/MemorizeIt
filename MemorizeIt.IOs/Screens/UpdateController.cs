using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MemorizeIt.MemoryStorage;
using FileMemoryStorage;
using System.Threading.Tasks;
using MemorizeIt.MemorySourceSupplier;
using GoogleMemorySupplier;
using System.Drawing;

namespace MemorizeIt.IOs.Screens
{
	public class UpdateController:UIViewController
	{
		private readonly IMemoryStorage store;
		private UIButton btnUpdate;
		private LoadingOverlay loadingOverlay;
		public UpdateController(IMemoryStorage store):base()
		{
			Initialize();
			this.store = store;
		}
		protected void Initialize()
		{

			this.TabBarItem =new UITabBarItem (UITabBarSystemItem.Downloads, 1);


		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();	
			View.BackgroundColor = UIColor.White;
			btnUpdate = UIButton.FromType(UIButtonType.RoundedRect);
			btnUpdate.SetTitle ("Update from my Google Drive", UIControlState.Normal);
			btnUpdate.TouchUpInside += (sender,e) => Upload ();
			View.AddSubview (btnUpdate);


			AlignMyGoogleButton();

		}

		protected void Upload()
		{

			var dialod = new UIAlertView("Enter credentials", "", null, "Cancel", null);

			dialod.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
			dialod.GetTextField (0).KeyboardType = UIKeyboardType.EmailAddress;

			dialod.AddButton("Upload");

			dialod.Show();

			dialod.Clicked += (sender, e) =>
			{
				if (e.ButtonIndex == 0)
					return;
				loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				View.Add(loadingOverlay);
				var supplierParams = new string[]
				{"MemorizeIt", dialod.GetTextField(0).Text, dialod.GetTextField(1).Text};

				Task.Factory.StartNew(() =>
				                      {
					try
					{
						var supplier = CreateSourceSupplier(supplierParams);
						var data = supplier.Download();
						this.store.Store(data);
					}
					catch (Exception downloadException)
					{
						this.InvokeOnMainThread(() =>
						                        new UIAlertView("Error", downloadException.Message, null, "OK",
						                null).Show());
					}
					this.InvokeOnMainThread(() =>
					                        loadingOverlay.Hide());
				});
			};


		}

		void AlignMyGoogleButton ()
		{
			btnUpdate.SizeToFit ();
			btnUpdate.Frame = new RectangleF ((View.Frame.Width - btnUpdate.Frame.Width) / 2, 10, btnUpdate.Frame.Width, btnUpdate.Frame.Height);
		}
		
		protected IMemorySourceSupplier CreateSourceSupplier(params string[] supplierParameters)
		{
			return new GoogleMemorySourceSupplier(supplierParameters[0], supplierParameters[1], supplierParameters[2]);
			/*return new SimpleMemorySourceSupplier (new MemoryItem[]{
                new MemoryItem("q1","a1"),
                new MemoryItem("q2","a2"),
                new MemoryItem("q3","a3")
            });*/
		}

	}
}

